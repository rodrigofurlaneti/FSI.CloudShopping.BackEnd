namespace FSI.CloudShopping.Domain.Enums;

/// <summary>
/// Represents the status of an order throughout its lifecycle.
/// </summary>
public enum OrderStatus
{
    Pending = 0,
    Confirmed = 1,
    Processing = 2,
    Shipped = 3,
    Delivered = 4,
    Cancelled = 5,
    Refunded = 6
}
