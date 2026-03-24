namespace FSI.CloudShopping.Domain.Entities;

using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Enums;
using FSI.CloudShopping.Domain.ValueObjects;
using FSI.CloudShopping.Domain.Events;

/// <summary>
/// Aggregate root for Product. Represents a product in the e-commerce catalog.
/// </summary>
public class Product : AggregateRoot<int>
{
    public SKU Sku { get; private set; }
    public string Name { get; private set; }
    public Slug Slug { get; private set; }
    public string Description { get; private set; }
    public string ShortDescription { get; private set; }
    public Money Price { get; private set; }
    public Money? CompareAtPrice { get; private set; }
    public Money CostPrice { get; private set; }
    public int StockQuantity { get; private set; }
    public int ReservedQuantity { get; private set; }
    public int MinStockAlert { get; private set; }
    public Guid CategoryId { get; private set; }
    public ProductStatus Status { get; private set; }
    public string? ImageUrl { get; private set; }
    public double Weight { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsFeatured { get; private set; }

    // Navigations
    public Category? Category { get; private set; }
    private readonly List<ProductImage> _images = [];
    public IReadOnlyCollection<ProductImage> Images => _images.AsReadOnly();

    public Product(int id, SKU sku, string name, Slug slug, string description, string shortDescription,
        Money price, Money costPrice, int stockQuantity, int minStockAlert, Guid categoryId,
        ProductStatus status = ProductStatus.Active, string? imageUrl = null, double weight = 0,
        Money? compareAtPrice = null, bool isActive = true, bool isFeatured = false)
        : base(id)
    {
        Sku = sku;
        Name = name;
        Slug = slug;
        Description = description;
        ShortDescription = shortDescription;
        Price = price;
        CompareAtPrice = compareAtPrice;
        CostPrice = costPrice;
        StockQuantity = stockQuantity;
        ReservedQuantity = 0;
        MinStockAlert = minStockAlert;
        CategoryId = categoryId;
        Status = status;
        ImageUrl = imageUrl;
        Weight = weight;
        IsActive = isActive;
        IsFeatured = isFeatured;
    }

    protected Product() { }

    public static Product Create(SKU sku, string name, Slug slug, string description, string shortDescription,
        Money price, Money costPrice, int stockQuantity, int minStockAlert, Guid categoryId,
        string? imageUrl = null, double weight = 0, Money? compareAtPrice = null)
    {
        return new Product(0, sku, name, slug, description, shortDescription, price, costPrice,
            stockQuantity, minStockAlert, categoryId, ProductStatus.Active, imageUrl, weight, compareAtPrice, true, false);
    }

    public void Update(string name, Slug slug, string description, string shortDescription,
        Money price, Money costPrice, int minStockAlert, string? imageUrl = null, Money? compareAtPrice = null)
    {
        Name = name;
        Slug = slug;
        Description = description;
        ShortDescription = shortDescription;
        Price = price;
        CostPrice = costPrice;
        MinStockAlert = minStockAlert;
        ImageUrl = imageUrl;
        CompareAtPrice = compareAtPrice;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity to add must be greater than zero.");

        StockQuantity += quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity to remove must be greater than zero.");

        var availableStock = StockQuantity - ReservedQuantity;
        if (availableStock < quantity)
            throw new DomainException($"Insufficient stock. Available: {availableStock}, Requested: {quantity}");

        StockQuantity -= quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ReserveStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity to reserve must be greater than zero.");

        var availableStock = StockQuantity - ReservedQuantity;
        if (availableStock < quantity)
            throw new DomainException($"Insufficient available stock to reserve. Available: {availableStock}, Requested: {quantity}");

        ReservedQuantity += quantity;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new StockReservedEvent(Id, quantity));
    }

    public void ReleaseReservedStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity to release must be greater than zero.");

        if (ReservedQuantity < quantity)
            throw new DomainException($"Cannot release more reserved stock than available. Reserved: {ReservedQuantity}, Requested: {quantity}");

        ReservedQuantity -= quantity;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new StockReleasedEvent(Id, quantity));
    }

    public void UpdatePrice(Money newPrice)
    {
        Price = newPrice;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        Status = ProductStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        Status = ProductStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsFeatured()
    {
        IsFeatured = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UnmarkAsFeatured()
    {
        IsFeatured = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetStatus(ProductStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddImage(ProductImage image)
    {
        _images.Add(image);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveImage(Guid imageId)
    {
        var image = _images.FirstOrDefault(i => i.Id == imageId);
        if (image != null)
        {
            _images.Remove(image);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public int GetAvailableStock() => StockQuantity - ReservedQuantity;

    public decimal GetMargin() => Price.Amount - CostPrice.Amount;

    public decimal GetMarginPercentage()
    {
        if (CostPrice.Amount == 0)
            return 0;

        return (GetMargin() / CostPrice.Amount) * 100;
    }
}
