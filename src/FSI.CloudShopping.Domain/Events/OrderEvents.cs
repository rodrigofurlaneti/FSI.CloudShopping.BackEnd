namespace FSI.CloudShopping.Domain.Events;

using FSI.CloudShopping.Domain.Core;

public class OrderPlacedEvent : DomainEvent
{
    public Guid CustomerId { get; }
    public string OrderNumber { get; }
    public decimal TotalAmount { get; }

    public OrderPlacedEvent(Guid customerId, string orderNumber, decimal totalAmount)
    {
        CustomerId = customerId;
        OrderNumber = orderNumber;
        TotalAmount = totalAmount;
    }
}

public class OrderConfirmedEvent : DomainEvent
{
    public int OrderId { get; }
    public string OrderNumber { get; }
    public Guid CustomerId { get; }

    public OrderConfirmedEvent(int orderId, string orderNumber, Guid customerId)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        CustomerId = customerId;
    }
}

public class OrderCancelledEvent : DomainEvent
{
    public int OrderId { get; }
    public string OrderNumber { get; }
    public Guid CustomerId { get; }
    public string? Reason { get; }

    public OrderCancelledEvent(int orderId, string orderNumber, Guid customerId, string? reason = null)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        CustomerId = customerId;
        Reason = reason;
    }
}
