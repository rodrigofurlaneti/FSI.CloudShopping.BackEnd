namespace FSI.CloudShopping.Application.DTOs.Order
{
    public record OrderItemDTO(int ProductId, int Quantity, decimal UnitPrice, decimal SubTotal);
}
