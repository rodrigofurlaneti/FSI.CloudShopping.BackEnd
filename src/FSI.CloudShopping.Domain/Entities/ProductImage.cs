namespace FSI.CloudShopping.Domain.Entities;

using FSI.CloudShopping.Domain.Core;

/// <summary>
/// Entity representing an image for a product.
/// </summary>
public class ProductImage : Entity<Guid>
{
    public int ProductId { get; private set; }
    public string Url { get; private set; }
    public string? AltText { get; private set; }
    public int SortOrder { get; private set; }
    public bool IsPrimary { get; private set; }

    // Navigation
    public Product? Product { get; private set; }

    public ProductImage(Guid id, int productId, string url, string? altText = null, int sortOrder = 0, bool isPrimary = false)
        : base(id)
    {
        ProductId = productId;
        Url = url;
        AltText = altText;
        SortOrder = sortOrder;
        IsPrimary = isPrimary;
    }

    protected ProductImage() { }

    public void SetAsPrimary()
    {
        IsPrimary = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetAsSecondary()
    {
        IsPrimary = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateSortOrder(int order)
    {
        SortOrder = order;
        UpdatedAt = DateTime.UtcNow;
    }
}
