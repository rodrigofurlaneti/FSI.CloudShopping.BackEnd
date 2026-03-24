namespace FSI.CloudShopping.Application.DTOs;

using FSI.CloudShopping.Domain.Enums;

public class ProductDto
{
    public int Id { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? CompareAtPrice { get; set; }
    public decimal CostPrice { get; set; }
    public int StockQuantity { get; set; }
    public int ReservedQuantity { get; set; }
    public int AvailableStock { get; set; }
    public int MinStockAlert { get; set; }
    public Guid CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public double Weight { get; set; }
    public bool IsActive { get; set; }
    public bool IsFeatured { get; set; }
    public List<ProductImageDto> Images { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class ProductSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public decimal? CompareAtPrice { get; set; }
    public int AvailableStock { get; set; }
    public bool IsFeatured { get; set; }
}

public class ProductImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? AltText { get; set; }
    public int SortOrder { get; set; }
    public bool IsPrimary { get; set; }
}

public record CreateProductRequest(
    string Sku,
    string Name,
    string Slug,
    string Description,
    string ShortDescription,
    decimal Price,
    decimal CostPrice,
    int StockQuantity,
    int MinStockAlert,
    Guid CategoryId,
    string? ImageUrl = null,
    double Weight = 0,
    decimal? CompareAtPrice = null
);

public record UpdateProductRequest(
    string Name,
    string Slug,
    string Description,
    string ShortDescription,
    decimal Price,
    decimal CostPrice,
    int MinStockAlert,
    string? ImageUrl = null,
    decimal? CompareAtPrice = null
);

public record UpdateStockRequest(int Quantity);

public class ProductFilterRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public Guid? CategoryId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? SortBy { get; set; } = "name";
    public string? SortOrder { get; set; } = "asc";
}
