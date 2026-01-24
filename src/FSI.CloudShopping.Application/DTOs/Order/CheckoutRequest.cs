namespace FSI.CloudShopping.Application.DTOs.Order
{
    public record CheckoutRequest(int CustomerId, int ShippingAddressId);
}
