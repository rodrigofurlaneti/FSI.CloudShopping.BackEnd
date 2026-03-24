namespace FSI.CloudShopping.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Enums;
using FSI.CloudShopping.Domain.ValueObjects;
using FSI.CloudShopping.Infrastructure.Data;

public class ProductRepository : Repository<Product, int>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Product?> GetBySkuAsync(SKU sku, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Sku == sku, cancellationToken);
    }

    public async Task<Product?> GetBySlugAsync(Slug slug, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Slug == slug, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(p => p.CategoryId == categoryId && p.IsActive)
            .Include(p => p.Images)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetFeaturedAsync(int limit = 10, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(p => p.IsFeatured && p.IsActive)
            .Include(p => p.Images)
            .OrderBy(p => p.Name)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> SearchAsync(string searchTerm, int skip = 0, int take = 10, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(p => p.IsActive && (p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm)))
            .Include(p => p.Images)
            .OrderBy(p => p.Name)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<Product>, int)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? search = null,
        Guid? categoryId = null,
        string? status = null,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Include(p => p.Images)
            .AsQueryable();

        // Apply status filter (defaults to active-only when no status specified)
        if (!string.IsNullOrEmpty(status) && Enum.TryParse<ProductStatus>(status, ignoreCase: true, out var parsedStatus))
            query = query.Where(p => p.Status == parsedStatus);
        else if (string.IsNullOrEmpty(status))
            query = query.Where(p => p.IsActive);

        // Apply search filter
        if (!string.IsNullOrEmpty(search))
        {
            var lowerSearch = search.ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(lowerSearch)
                || p.Description.ToLower().Contains(lowerSearch));
        }

        // Apply category filter
        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        query = query.OrderBy(p => p.Name);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
