namespace FSI.CloudShopping.Domain.Entities;

using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
using FSI.CloudShopping.Domain.Events;

/// <summary>
/// Aggregate root for Cart. Represents a shopping cart for a customer session.
/// </summary>
public class Cart : AggregateRoot<int>
{
    public Guid CustomerId { get; private set; }
    public DateTime ExpiresAt { get; private set; }

    private readonly List<CartItem> _items = [];
    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

    public Cart(int id, Guid customerId) : base(id)
    {
        CustomerId = customerId;
        ExpiresAt = DateTime.UtcNow.AddDays(30);
    }

    protected Cart() { }

    public static Cart Create(Guid customerId)
    {
        return new Cart(0, customerId);
    }

    public void AddOrUpdateItem(int productId, string productName, string productImageUrl, string productSku,
        Quantity quantity, Money unitPrice)
    {
        var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);

        if (existingItem != null)
        {
            existingItem.UpdateQuantity(quantity);
        }
        else
        {
            var item = new CartItem(Guid.NewGuid(), Id, productId, productName, productImageUrl, productSku, quantity, unitPrice);
            _items.Add(item);
        }

        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveItem(int productId)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            _items.Remove(item);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void Clear()
    {
        _items.Clear();
        UpdatedAt = DateTime.UtcNow;
    }

    public void Merge(Cart otherCart)
    {
        if (otherCart.Id == Id)
            throw new DomainException("Cannot merge cart with itself.");

        foreach (var item in otherCart.Items)
        {
            AddOrUpdateItem(item.ProductId, item.ProductName, item.ProductImageUrl, item.ProductSku,
                new Quantity(item.Quantity), item.UnitPrice);
        }

        RaiseDomainEvent(new CartMergedEvent(Id, CustomerId, otherCart.CustomerId, _items.Count));
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsExpired => DateTime.UtcNow > ExpiresAt;

    public Money GetTotal()
    {
        if (_items.Count == 0)
            return new Money(0);

        var total = _items.Aggregate(
            new Money(0),
            (acc, item) => acc + item.Subtotal);

        return total;
    }

    public int GetItemCount() => _items.Sum(i => i.Quantity);

    public void ExtendExpiration(int daysFromNow = 30)
    {
        ExpiresAt = DateTime.UtcNow.AddDays(daysFromNow);
        UpdatedAt = DateTime.UtcNow;
    }
}
