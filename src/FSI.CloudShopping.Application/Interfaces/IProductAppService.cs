using FSI.CloudShopping.Application.DTOs.Product;
namespace FSI.CloudShopping.Application.Interfaces
{
    public interface IProductAppService : IBaseAppService<ProductDTO>
    {
        Task<ProductDTO?> GetBySkuAsync(string sku);
        Task UpdateStockAsync(int productId, int quantity);
        Task<IEnumerable<ProductDTO>> GetByCategoryIdAsync(int categoryId);
    }
}