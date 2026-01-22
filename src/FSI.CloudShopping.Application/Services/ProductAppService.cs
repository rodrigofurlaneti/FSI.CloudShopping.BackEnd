using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Application.Mappings;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;

namespace FSI.CloudShopping.Application.Services
{
    public class ProductAppService : BaseAppService<ProductDTO, Product>, IProductAppService
    {
        private readonly IProductRepository _productRepository;

        public ProductAppService(IProductRepository repository) : base(repository)
        {
            _productRepository = repository;
        }

        public async Task<ProductDTO?> GetBySkuAsync(string sku)
        {
            var product = await _productRepository.GetBySkuAsync(new SKU(sku));
            return product == null ? null : MapToDto(product);
        }

        public async Task UpdateStockAsync(int productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                throw new KeyNotFoundException($"Produto com ID {productId} não encontrado.");

            product.UpdateStock(new Quantity(quantity));

            await _productRepository.UpdateAsync(product);
            await _productRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductDTO>> GetByCategoryIdAsync(int categoryId)
        {
            var products = await _productRepository.GetByCategoryIdAsync(categoryId);
            return products.Select(MapToDto);
        }
        protected override Product MapToEntity(ProductDTO dto) => ProductMapping.MapToEntity(dto);
        protected override ProductDTO MapToDto(Product entity) => ProductMapping.MapToDto(entity);
    }
}