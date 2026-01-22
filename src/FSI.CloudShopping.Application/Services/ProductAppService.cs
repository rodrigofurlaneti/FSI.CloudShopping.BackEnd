using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Application.Interfaces;
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
            if (product == null) throw new ApplicationException("Produto não encontrado.");
            product.UpdateStock(new Quantity(quantity));
            await _productRepository.UpdateAsync(product);
            await _productRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductDTO>> GetByCategoryIdAsync(int categoryId)
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(MapToDto);
        }

        protected override Product MapToEntity(ProductDTO dto)
        {
            return new Product(
                new SKU(dto.Sku),
                dto.Name,
                new Money(dto.Price),
                new Quantity(dto.Stock)
            );
        }

        protected override ProductDTO MapToDto(Product entity)
        {
            return new ProductDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Sku = entity.Sku.Code,
                Price = entity.Price.Value,
                Stock = entity.Stock.Value
            };
        }
    }
}