namespace FSI.CloudShopping.Domain.Events;

using FSI.CloudShopping.Domain.Core;

public class CartMergedEvent : DomainEvent
{
    public int CartId { get; }
    public Guid CustomerId { get; }
    public Guid OtherCustomerId { get; }
    public int ItemCount { get; }

    public CartMergedEvent(int cartId, Guid customerId, Guid otherCustomerId, int itemCount)
    {
        CartId = cartId;
        CustomerId = customerId;
        OtherCustomerId = otherCustomerId;
        ItemCount = itemCount;
    }
}
