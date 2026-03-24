namespace FSI.CloudShopping.Domain.Enums;

/// <summary>
/// Represents the status of a payment in the system.
/// </summary>
public enum PaymentStatus
{
    Pending = 0,
    Authorized = 1,
    Captured = 2,
    Failed = 3,
    Refunded = 4,
    Cancelled = 5
}
