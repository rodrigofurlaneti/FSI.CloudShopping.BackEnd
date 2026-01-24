namespace FSI.CloudShopping.Application.DTOs.Cart
{
    public record CartItemDTO(int ProductId, string ProductName, int Quantity, decimal UnitPrice, decimal SubTotal);
}
