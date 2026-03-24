namespace FSI.CloudShopping.Domain.Interfaces;

using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;

public interface IProductRepository : IRepository<Product, int>
{
    Task<Product?> GetBySkuAsync(SKU sku, CancellationToken cancellationToken = default);
    Task<Product?> GetBySlugAsync(Slug slug, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetFeaturedAsync(int limit = 10, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> SearchAsync(string searchTerm, int skip = 0, int take = 10, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Product>, int)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? search = null,
        Guid? categoryId = null,
        string? status = null,
        CancellationToken cancellationToken = default);
}
