namespace FSI.CloudShopping.Application.DTOs.Order
{
    public record CheckoutRequest(Guid CustomerId, Guid ShippingAddressId);
}
