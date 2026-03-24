namespace FSI.CloudShopping.Domain.Entities;

using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Enums;
using FSI.CloudShopping.Domain.ValueObjects;

/// <summary>
/// Aggregate root for Payment. Represents a payment for an order.
/// </summary>
public class Payment : AggregateRoot<int>
{
    public int OrderId { get; private set; }
    public PaymentMethod Method { get; private set; }
    public Money Amount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public string? TransactionId { get; private set; }
    public string? GatewayResponse { get; private set; }
    public DateTime? ProcessedAt { get; private set; }
    public string? FailureReason { get; private set; }
    public int RetryCount { get; private set; }

    // Navigation
    public Order? Order { get; private set; }

    public Payment(int id, int orderId, PaymentMethod method, Money amount) : base(id)
    {
        OrderId = orderId;
        Method = method;
        Amount = amount;
        Status = PaymentStatus.Pending;
        RetryCount = 0;
    }

    protected Payment() { }

    public static Payment Create(int orderId, PaymentMethod method, Money amount)
    {
        return new Payment(0, orderId, method, amount);
    }

    public void Authorize(string transactionId, string? gatewayResponse = null)
    {
        if (Status != PaymentStatus.Pending)
            throw new DomainException($"Cannot authorize payment with status {Status}");

        Status = PaymentStatus.Authorized;
        TransactionId = transactionId;
        GatewayResponse = gatewayResponse;
        ProcessedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new PaymentAuthorizedEvent(Id, OrderId, Amount.Amount));
    }

    public void Capture(string? gatewayResponse = null)
    {
        if (Status != PaymentStatus.Authorized)
            throw new DomainException($"Cannot capture payment with status {Status}");

        Status = PaymentStatus.Captured;
        GatewayResponse = gatewayResponse;
        ProcessedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new PaymentCapturedEvent(Id, OrderId, Amount.Amount));
    }

    public void Fail(string reason)
    {
        if (Status == PaymentStatus.Captured || Status == PaymentStatus.Refunded)
            throw new DomainException($"Cannot fail payment with status {Status}");

        Status = PaymentStatus.Failed;
        FailureReason = reason;
        ProcessedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new PaymentFailedEvent(Id, OrderId, reason));
    }

    public void Refund(string? reason = null)
    {
        if (Status != PaymentStatus.Captured)
            throw new DomainException($"Cannot refund payment with status {Status}");

        Status = PaymentStatus.Refunded;
        FailureReason = reason;
        ProcessedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void IncrementRetry()
    {
        RetryCount++;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool CanRetry() => RetryCount < 3 && Status == PaymentStatus.Failed;
}
