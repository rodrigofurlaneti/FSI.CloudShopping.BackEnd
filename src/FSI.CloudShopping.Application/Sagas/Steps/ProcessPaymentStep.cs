namespace FSI.CloudShopping.Application.Sagas.Steps;

using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Enums;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;
using FSI.CloudShopping.Domain.Sagas;
using Microsoft.Extensions.Logging;

/// <summary>
/// SAGA Step 2 — Process Payment.
/// Calls the payment gateway and persists the Payment aggregate.
/// Compensation: issues a refund if order confirmation (Step 3) fails.
/// </summary>
public sealed class ProcessPaymentStep : ISagaStep<OrderCheckoutSagaState>
{
    private readonly IPaymentGatewayService _gateway;
    private readonly IPaymentRepository _paymentRepository;
    private readonly ILogger<ProcessPaymentStep> _logger;

    public string StepName => "ProcessPayment";

    public ProcessPaymentStep(
        IPaymentGatewayService gateway,
        IPaymentRepository paymentRepository,
        ILogger<ProcessPaymentStep> logger)
    {
        _gateway = gateway;
        _paymentRepository = paymentRepository;
        _logger = logger;
    }

    public async Task ExecuteAsync(OrderCheckoutSagaState state, CancellationToken cancellationToken = default)
    {
        // Calculate total from items (order not yet persisted at this point)
        var total = state.Items.Sum(i => i.UnitPrice * i.Quantity);
        var amount = new Money(total);

        var payment = Payment.Create(0 /* temp, order not yet created */, state.PaymentMethod, amount);
        await _paymentRepository.AddAsync(payment, cancellationToken);
        await _paymentRepository.SaveChangesAsync(cancellationToken);

        state.PaymentId = payment.Id;

        // Call payment gateway
        var result = await _gateway.ChargeAsync(payment, cancellationToken);
        if (!result.Success)
        {
            payment.Fail(result.ErrorMessage ?? "Gateway rejection");
            await _paymentRepository.UpdateAsync(payment, cancellationToken);
            await _paymentRepository.SaveChangesAsync(cancellationToken);
            throw new InvalidOperationException($"Payment failed: {result.ErrorMessage}");
        }

        payment.Authorize(result.TransactionId!, result.GatewayResponse);
        payment.Capture(result.GatewayResponse);
        await _paymentRepository.UpdateAsync(payment, cancellationToken);
        await _paymentRepository.SaveChangesAsync(cancellationToken);

        state.TransactionId = result.TransactionId;
        state.PaymentProcessed = true;
        _logger.LogInformation("Payment {PaymentId} captured, TransactionId: {Txn}", payment.Id, result.TransactionId);
    }

    public async Task CompensateAsync(OrderCheckoutSagaState state, CancellationToken cancellationToken = default)
    {
        if (!state.PaymentId.HasValue) return;

        _logger.LogWarning("Compensating: refunding payment {PaymentId} for SAGA {SagaId}", state.PaymentId, state.SagaId);

        var payment = await _paymentRepository.GetByIdAsync(state.PaymentId.Value, cancellationToken);
        if (payment is null) return;

        payment.Refund("Saga compensation — order could not be confirmed.");
        await _paymentRepository.UpdateAsync(payment, cancellationToken);
        await _paymentRepository.SaveChangesAsync(cancellationToken);

        state.PaymentProcessed = false;
    }
}
