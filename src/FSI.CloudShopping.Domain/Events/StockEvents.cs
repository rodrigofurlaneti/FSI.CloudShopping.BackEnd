namespace FSI.CloudShopping.Domain.Events;

using FSI.CloudShopping.Domain.Core;

public class StockReservedEvent : DomainEvent
{
    public int ProductId { get; }
    public int Quantity { get; }

    public StockReservedEvent(int productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }
}

public class StockReleasedEvent : DomainEvent
{
    public int ProductId { get; }
    public int Quantity { get; }

    public StockReleasedEvent(int productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }
}
