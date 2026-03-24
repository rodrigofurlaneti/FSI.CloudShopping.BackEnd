namespace FSI.CloudShopping.Application.Commands.Order;

using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Domain.Enums;
using MediatR;

/// <summary>
/// CQRS Command — triggers the Order Checkout SAGA.
/// Encapsulates all data required to start the distributed transaction.
/// </summary>
public sealed record CheckoutOrderCommand : IRequest<Result<CheckoutOrderResult>>
{
    public Guid CustomerId { get; init; }
    public Guid ShippingAddressId { get; init; }
    public PaymentMethod PaymentMethod { get; init; }
    public string? CouponCode { get; init; }
    public string? Notes { get; init; }
    public List<CheckoutItemDto> Items { get; init; } = [];
}

public sealed record CheckoutItemDto(
    int ProductId,
    string ProductName,
    string ProductSku,
    int Quantity,
    decimal UnitPrice);

public sealed record CheckoutOrderResult(
    Guid SagaId,
    int OrderId,
    string OrderNumber,
    string Status,
    string? FailureReason = null);
