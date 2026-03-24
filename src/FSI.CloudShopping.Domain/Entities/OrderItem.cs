namespace FSI.CloudShopping.Domain.Entities;

using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;

/// <summary>
/// Entity representing an item in an order.
/// </summary>
public class OrderItem : Entity<Guid>
{
    public int OrderId { get; private set; }
    public int ProductId { get; private set; }
    public string ProductName { get; private set; }
    public string ProductSku { get; private set; }
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; }
    public Money Discount { get; private set; }

    public Money Subtotal => (UnitPrice * Quantity) - Discount;

    // Navigation
    public Order? Order { get; private set; }

    public OrderItem(Guid id, int orderId, int productId, string productName, string productSku,
        int quantity, Money unitPrice, Money discount) : base(id)
    {
        OrderId = orderId;
        ProductId = productId;
        ProductName = productName;
        ProductSku = productSku;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = discount;
    }

    protected OrderItem() { }
}
