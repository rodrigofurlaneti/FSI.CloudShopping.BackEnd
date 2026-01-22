namespace FSI.CloudShopping.Application.DTOs
{
    public record CartItemDTO(int ProductId, string ProductName, int Quantity, decimal UnitPrice, decimal TotalPrice);
}
