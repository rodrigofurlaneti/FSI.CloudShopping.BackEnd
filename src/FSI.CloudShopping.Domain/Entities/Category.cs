namespace FSI.CloudShopping.Domain.Entities;

using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;

/// <summary>
/// Aggregate root for Category. Represents a product category.
/// </summary>
public class Category : AggregateRoot<Guid>
{
    public string Name { get; private set; }
    public Slug Slug { get; private set; }
    public string? Description { get; private set; }
    public Guid? ParentCategoryId { get; private set; }
    public bool IsActive { get; private set; }
    public int SortOrder { get; private set; }
    public string? ImageUrl { get; private set; }

    // Navigation
    public Category? ParentCategory { get; private set; }
    private readonly List<Category> _childCategories = [];
    public IReadOnlyCollection<Category> ChildCategories => _childCategories.AsReadOnly();

    public Category(Guid id, string name, Slug slug, string? description = null, Guid? parentCategoryId = null, bool isActive = true, int sortOrder = 0, string? imageUrl = null)
        : base(id)
    {
        Name = name;
        Slug = slug;
        Description = description;
        ParentCategoryId = parentCategoryId;
        IsActive = isActive;
        SortOrder = sortOrder;
        ImageUrl = imageUrl;
    }

    protected Category() { }

    public static Category Create(string name, Slug slug, string? description = null, Guid? parentCategoryId = null, string? imageUrl = null)
    {
        return new Category(Guid.NewGuid(), name, slug, description, parentCategoryId, true, 0, imageUrl);
    }

    public void Update(string name, Slug slug, string? description = null, string? imageUrl = null)
    {
        Name = name;
        Slug = slug;
        Description = description;
        ImageUrl = imageUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetSortOrder(int order)
    {
        SortOrder = order;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddChildCategory(Category childCategory)
    {
        if (childCategory.ParentCategoryId != Id)
            throw new DomainException("Child category must have this category as parent.");

        _childCategories.Add(childCategory);
        UpdatedAt = DateTime.UtcNow;
    }
}
