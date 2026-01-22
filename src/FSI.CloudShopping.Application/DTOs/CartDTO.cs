namespace FSI.CloudShopping.Application.DTOs
{
    public record CartDTO
    {
        public int Id { get; init; }
        public string? VisitorToken { get; init; }
        public int? CustomerId { get; init; }
        public List<CartItemDTO> Items { get; init; } = new();
        public decimal TotalAmount { get; init; }
    }
}
