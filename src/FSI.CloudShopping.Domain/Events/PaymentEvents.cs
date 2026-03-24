namespace FSI.CloudShopping.Domain.Events;

using FSI.CloudShopping.Domain.Core;

public class PaymentAuthorizedEvent : DomainEvent
{
    public int PaymentId { get; }
    public int OrderId { get; }
    public decimal Amount { get; }

    public PaymentAuthorizedEvent(int paymentId, int orderId, decimal amount)
    {
        PaymentId = paymentId;
        OrderId = orderId;
        Amount = amount;
    }
}

public class PaymentCapturedEvent : DomainEvent
{
    public int PaymentId { get; }
    public int OrderId { get; }
    public decimal Amount { get; }

    public PaymentCapturedEvent(int paymentId, int orderId, decimal amount)
    {
        PaymentId = paymentId;
        OrderId = orderId;
        Amount = amount;
    }
}

public class PaymentFailedEvent : DomainEvent
{
    public int PaymentId { get; }
    public int OrderId { get; }
    public string Reason { get; }

    public PaymentFailedEvent(int paymentId, int orderId, string reason)
    {
        PaymentId = paymentId;
        OrderId = orderId;
        Reason = reason;
    }
}
