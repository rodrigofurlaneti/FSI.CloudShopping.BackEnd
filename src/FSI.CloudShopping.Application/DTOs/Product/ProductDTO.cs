namespace FSI.CloudShopping.Application.DTOs.Product
{
    public record ProductDTO
    {
        public int Id { get; init; }
        public string Sku { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
        public int StockQuantity { get; init; }
        public int CategoryId { get; init; }
        public bool IsActive { get; init; }
    }
}