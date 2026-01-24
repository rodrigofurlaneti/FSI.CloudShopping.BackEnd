namespace FSI.CloudShopping.Application.DTOs.Cart
{
    public record CartDTO
    {
        public int Id { get; init; }
        public int CustomerId { get; init; }
        public decimal Total { get; init; }
        public List<CartItemDTO> Items { get; init; } = new();
    }
}
