namespace FSI.CloudShopping.Domain.Entities;

using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Enums;
using FSI.CloudShopping.Domain.ValueObjects;

/// <summary>
/// Aggregate root for Order. Represents a customer order.
/// </summary>
public class Order : AggregateRoot<int>
{
    public string OrderNumber { get; private set; }
    public Guid CustomerId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public OrderStatus Status { get; private set; }
    public Guid ShippingAddressId { get; private set; }
    public Money SubTotal { get; private set; }
    public Money DiscountAmount { get; private set; }
    public Money ShippingCost { get; private set; }
    public Money TotalAmount { get; private set; }
    public string? CouponCode { get; private set; }
    public string? TrackingNumber { get; private set; }
    public string? Notes { get; private set; }

    private readonly List<OrderItem> _items = [];
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    // Navigation
    public Customer? Customer { get; private set; }
    public Address? ShippingAddress { get; private set; }

    public Order(int id, string orderNumber, Guid customerId, DateTime orderDate, Guid shippingAddressId,
        Money subTotal, Money discountAmount, Money shippingCost, Money totalAmount,
        string? couponCode = null, string? trackingNumber = null, string? notes = null)
        : base(id)
    {
        OrderNumber = orderNumber;
        CustomerId = customerId;
        OrderDate = orderDate;
        Status = OrderStatus.Pending;
        ShippingAddressId = shippingAddressId;
        SubTotal = subTotal;
        DiscountAmount = discountAmount;
        ShippingCost = shippingCost;
        TotalAmount = totalAmount;
        CouponCode = couponCode;
        TrackingNumber = trackingNumber;
        Notes = notes;
    }

    protected Order() { }

    public static Order Create(Guid customerId, Guid shippingAddressId, Money subTotal,
        Money discountAmount, Money shippingCost, string? couponCode = null, string? notes = null)
    {
        var totalAmount = (subTotal + shippingCost) - discountAmount;
        var orderNumber = GenerateOrderNumber();
        var order = new Order(0, orderNumber, customerId, DateTime.UtcNow, shippingAddressId,
            subTotal, discountAmount, shippingCost, totalAmount, couponCode, null, notes);

        order.RaiseDomainEvent(new OrderPlacedEvent(order.CustomerId, orderNumber, totalAmount.Amount));
        return order;
    }

    private static string GenerateOrderNumber()
    {
        return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }

    public void AddItem(int productId, string productName, string productSku, int quantity,
        Money unitPrice, Money discount)
    {
        var item = new OrderItem(Guid.NewGuid(), Id, productId, productName, productSku,
            quantity, unitPrice, discount);
        _items.Add(item);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException($"Cannot confirm order with status {Status}");

        Status = OrderStatus.Confirmed;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new OrderConfirmedEvent(Id, OrderNumber, CustomerId));
    }

    public void StartProcessing()
    {
        if (Status != OrderStatus.Confirmed)
            throw new DomainException($"Cannot process order with status {Status}");

        Status = OrderStatus.Processing;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Ship(string trackingNumber)
    {
        if (Status != OrderStatus.Processing)
            throw new DomainException($"Cannot ship order with status {Status}");

        TrackingNumber = trackingNumber;
        Status = OrderStatus.Shipped;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deliver()
    {
        if (Status != OrderStatus.Shipped)
            throw new DomainException($"Cannot deliver order with status {Status}");

        Status = OrderStatus.Delivered;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel(string? reason = null)
    {
        if (Status == OrderStatus.Delivered || Status == OrderStatus.Cancelled || Status == OrderStatus.Refunded)
            throw new DomainException($"Cannot cancel order with status {Status}");

        Status = OrderStatus.Cancelled;
        Notes = reason ?? Notes;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new OrderCancelledEvent(Id, OrderNumber, CustomerId, reason));
    }

    public void Refund()
    {
        if (Status != OrderStatus.Delivered)
            throw new DomainException("Only delivered orders can be refunded");

        Status = OrderStatus.Refunded;
        UpdatedAt = DateTime.UtcNow;
    }

    public void CalculateTotals()
    {
        var itemsSubtotal = _items.Aggregate(
            new Money(0),
            (acc, item) => acc + (item.UnitPrice * item.Quantity) - item.Discount);

        SubTotal = itemsSubtotal;
        TotalAmount = (SubTotal + ShippingCost) - DiscountAmount;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ApplyCoupon(string couponCode)
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException("Coupon can only be applied to pending orders");

        CouponCode = couponCode;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool CanBeCancelled => Status is OrderStatus.Pending or OrderStatus.Confirmed or OrderStatus.Processing;

    public bool CanBeRefunded => Status == OrderStatus.Delivered;
}
