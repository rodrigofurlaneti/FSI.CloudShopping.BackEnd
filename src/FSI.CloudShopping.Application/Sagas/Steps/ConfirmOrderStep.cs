namespace FSI.CloudShopping.Application.Sagas.Steps;

using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Sagas;
using FSI.CloudShopping.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

/// <summary>
/// SAGA Step 3 — Confirm Order.
/// Creates and persists the Order aggregate, then links the Payment to it.
/// Compensation: cancels the order if the notification step fails.
/// </summary>
public sealed class ConfirmOrderStep : ISagaStep<OrderCheckoutSagaState>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly ILogger<ConfirmOrderStep> _logger;

    public string StepName => "ConfirmOrder";

    public ConfirmOrderStep(
        IOrderRepository orderRepository,
        IPaymentRepository paymentRepository,
        ILogger<ConfirmOrderStep> logger)
    {
        _orderRepository = orderRepository;
        _paymentRepository = paymentRepository;
        _logger = logger;
    }

    public async Task ExecuteAsync(OrderCheckoutSagaState state, CancellationToken cancellationToken = default)
    {
        var subTotal = state.Items.Sum(i => i.UnitPrice * i.Quantity);
        var order = Order.Create(
            customerId: state.CustomerId,
            shippingAddressId: state.ShippingAddressId,
            subTotal: new Money(subTotal),
            discountAmount: new Money(0),
            shippingCost: new Money(0),
            couponCode: state.CouponCode,
            notes: state.Notes);

        foreach (var item in state.Items)
        {
            order.AddItem(
                productId: item.ProductId,
                productName: item.ProductName,
                productSku: item.ProductSku,
                quantity: item.Quantity,
                unitPrice: new Money(item.UnitPrice),
                discount: new Money(0));
        }

        order.Confirm();

        await _orderRepository.AddAsync(order, cancellationToken);
        await _orderRepository.SaveChangesAsync(cancellationToken);

        state.OrderId = order.Id;
        state.OrderNumber = order.OrderNumber;
        state.OrderConfirmed = true;

        _logger.LogInformation("Order {OrderNumber} confirmed (Id: {OrderId})", order.OrderNumber, order.Id);
    }

    public async Task CompensateAsync(OrderCheckoutSagaState state, CancellationToken cancellationToken = default)
    {
        if (!state.OrderId.HasValue) return;

        _logger.LogWarning("Compensating: cancelling order {OrderId} for SAGA {SagaId}", state.OrderId, state.SagaId);

        var order = await _orderRepository.GetByIdAsync(state.OrderId.Value, cancellationToken);
        if (order is null) return;

        order.Cancel("Saga compensation — checkout could not be completed.");
        await _orderRepository.UpdateAsync(order, cancellationToken);
        await _orderRepository.SaveChangesAsync(cancellationToken);

        state.OrderConfirmed = false;
    }
}
