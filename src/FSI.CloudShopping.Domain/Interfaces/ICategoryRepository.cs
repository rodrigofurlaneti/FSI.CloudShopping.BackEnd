namespace FSI.CloudShopping.Domain.Interfaces;

using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;

public interface ICategoryRepository : IRepository<Category, Guid>
{
    Task<Category?> GetBySlugAsync(Slug slug, CancellationToken cancellationToken = default);
    Task<Category?> GetWithChildrenAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Category>> GetRootCategoriesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Category>> GetByParentAsync(Guid parentCategoryId, CancellationToken cancellationToken = default);
}
