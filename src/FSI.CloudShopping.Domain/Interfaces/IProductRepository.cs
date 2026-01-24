using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product?> GetBySkuAsync(SKU sku);
        Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Product>> GetAvailableProductsAsync();
        Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold);
    }
}
