namespace FSI.CloudShopping.Application.DTOs
{
    public record ProductDTO
    {
        public int Id { get; init; }
        public string Sku { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public int Stock { get; init; }
        public bool IsActive { get; init; }
    }
}