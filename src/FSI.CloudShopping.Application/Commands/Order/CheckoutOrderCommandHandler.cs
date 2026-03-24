namespace FSI.CloudShopping.Application.Commands.Order;

using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.Sagas;
using FSI.CloudShopping.Domain.Sagas;
using MediatR;
using Microsoft.Extensions.Logging;

/// <summary>
/// CQRS Handler — receives CheckoutOrderCommand and delegates execution
/// to the OrderCheckoutSagaOrchestrator, keeping the handler thin and
/// decoupled from orchestration logic.
/// </summary>
public sealed class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, Result<CheckoutOrderResult>>
{
    private readonly OrderCheckoutSagaOrchestrator _saga;
    private readonly ILogger<CheckoutOrderCommandHandler> _logger;

    public CheckoutOrderCommandHandler(
        OrderCheckoutSagaOrchestrator saga,
        ILogger<CheckoutOrderCommandHandler> logger)
    {
        _saga = saga;
        _logger = logger;
    }

    public async Task<Result<CheckoutOrderResult>> Handle(
        CheckoutOrderCommand command,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Checkout initiated for customer {CustomerId}", command.CustomerId);

        var state = new OrderCheckoutSagaState
        {
            CustomerId = command.CustomerId,
            ShippingAddressId = command.ShippingAddressId,
            PaymentMethod = command.PaymentMethod,
            CouponCode = command.CouponCode,
            Notes = command.Notes,
            Items = command.Items
                .Select(i => new OrderItemInput(
                    i.ProductId, i.ProductName, i.ProductSku, i.Quantity, i.UnitPrice))
                .ToList()
        };

        var result = await _saga.ExecuteAsync(state, cancellationToken);

        if (result.Status == SagaStatus.Completed)
        {
            return Result<CheckoutOrderResult>.Success(
                new CheckoutOrderResult(
                    SagaId: result.SagaId,
                    OrderId: result.OrderId!.Value,
                    OrderNumber: result.OrderNumber!,
                    Status: "Completed"));
        }

        _logger.LogWarning("Checkout SAGA {SagaId} failed: {Reason}", result.SagaId, result.FailureReason);
        return Result<CheckoutOrderResult>.Failure(
            new CheckoutOrderResult(
                SagaId: result.SagaId,
                OrderId: 0,
                OrderNumber: string.Empty,
                Status: result.Status.ToString(),
                FailureReason: result.FailureReason),
            result.FailureReason ?? "Checkout could not be completed.");
    }
}
