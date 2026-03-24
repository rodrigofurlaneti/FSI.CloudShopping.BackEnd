namespace FSI.CloudShopping.Application.Sagas;

using FSI.CloudShopping.Domain.Enums;
using FSI.CloudShopping.Domain.Sagas;
using FSI.CloudShopping.Domain.ValueObjects;

/// <summary>
/// Shared state for the Order Checkout SAGA.
/// Carries all data needed across the four steps:
///   1. Reserve Stock
///   2. Process Payment
///   3. Confirm Order
///   4. Send Notification
/// </summary>
public sealed class OrderCheckoutSagaState : ISagaState
{
    // ── ISagaState ────────────────────────────────────────────────────────────
    public Guid SagaId { get; } = Guid.NewGuid();
    public SagaStatus Status { get; set; } = SagaStatus.NotStarted;
    public string? FailureReason { get; set; }
    public DateTime StartedAt { get; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    // ── Input ─────────────────────────────────────────────────────────────────
    public Guid CustomerId { get; init; }
    public Guid ShippingAddressId { get; init; }
    public string? CouponCode { get; init; }
    public string? Notes { get; init; }
    public PaymentMethod PaymentMethod { get; init; }
    public List<OrderItemInput> Items { get; init; } = [];

    // ── Step Results ──────────────────────────────────────────────────────────

    // Step 1 — Reserve Stock
    public bool StockReserved { get; set; }
    public List<Guid> ReservedStockIds { get; set; } = [];

    // Step 2 — Process Payment
    public int? PaymentId { get; set; }
    public string? TransactionId { get; set; }
    public bool PaymentProcessed { get; set; }

    // Step 3 — Confirm Order
    public int? OrderId { get; set; }
    public string? OrderNumber { get; set; }
    public bool OrderConfirmed { get; set; }

    // Step 4 — Send Notification
    public bool NotificationSent { get; set; }
}

/// <summary>Line item input for the checkout saga.</summary>
public sealed record OrderItemInput(
    int ProductId,
    string ProductName,
    string ProductSku,
    int Quantity,
    decimal UnitPrice);
