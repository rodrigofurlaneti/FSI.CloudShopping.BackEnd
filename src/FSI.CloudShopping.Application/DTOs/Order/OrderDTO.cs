namespace FSI.CloudShopping.Application.DTOs.Order
{
    public record OrderDTO
    {
        public int Id { get; init; }
        public int CustomerId { get; init; }
        public string Status { get; init; }
        public decimal TotalAmount { get; init; }
        public DateTime OrderDate { get; init; }
        public List<OrderItemDTO> Items { get; init; } = new();
    }
}
