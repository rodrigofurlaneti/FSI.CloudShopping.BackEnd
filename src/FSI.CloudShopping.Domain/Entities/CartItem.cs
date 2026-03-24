namespace FSI.CloudShopping.Domain.Entities;

using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;

/// <summary>
/// Entity representing an item in a shopping cart.
/// </summary>
public class CartItem : Entity<Guid>
{
    public int CartId { get; private set; }
    public int ProductId { get; private set; }
    public string ProductName { get; private set; }
    public string ProductImageUrl { get; private set; }
    public string ProductSku { get; private set; }
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; }

    public Money Subtotal => UnitPrice * Quantity;

    // Navigation
    public Cart? Cart { get; private set; }

    public CartItem(Guid id, int cartId, int productId, string productName, string productImageUrl,
        string productSku, Quantity quantity, Money unitPrice) : base(id)
    {
        CartId = cartId;
        ProductId = productId;
        ProductName = productName;
        ProductImageUrl = productImageUrl;
        ProductSku = productSku;
        Quantity = quantity.Value;
        UnitPrice = unitPrice;
    }

    protected CartItem() { }

    public void UpdateQuantity(Quantity quantity)
    {
        Quantity = quantity.Value;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateUnitPrice(Money price)
    {
        UnitPrice = price;
        UpdatedAt = DateTime.UtcNow;
    }
}
