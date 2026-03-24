namespace FSI.CloudShopping.Application.DTOs;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public bool IsActive { get; set; }
    public int SortOrder { get; set; }
    public string? ImageUrl { get; set; }
    public List<CategoryDto> ChildCategories { get; set; } = [];
}

public record CreateCategoryRequest(string Name, string Slug, string? Description = null, Guid? ParentCategoryId = null, string? ImageUrl = null);

public record UpdateCategoryRequest(string Name, string Slug, string? Description = null, string? ImageUrl = null);
