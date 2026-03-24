namespace FSI.CloudShopping.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;
using FSI.CloudShopping.Infrastructure.Data;

public class CategoryRepository : Repository<Category, Guid>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Category?> GetBySlugAsync(Slug slug, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(c => c.Slug == slug, cancellationToken);
    }

    public async Task<Category?> GetWithChildrenAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(c => c.ChildCategories)
            .FirstOrDefaultAsync(c => c.Id == categoryId, cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetRootCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(c => c.ParentCategoryId == null)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetByParentAsync(Guid parentCategoryId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(c => c.ParentCategoryId == parentCategoryId)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }
}
