using AutoMapper;
using FSI.CloudShopping.Application.DTOs.Product;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;

namespace FSI.CloudShopping.Application.Services
{
    public class ProductAppService : BaseAppService<Product, ProductDTO>, IProductAppService
    {
        private readonly IProductRepository _productRepository;

        public ProductAppService(IProductRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
            _productRepository = repository;
        }

        public async Task<ProductDTO?> GetBySkuAsync(string sku)
        {
            var product = await _productRepository.GetBySkuAsync(new SKU(sku));
            return Mapper.Map<ProductDTO>(product);
        }

        public async Task UpdateStockAsync(int productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) throw new DomainException("Produto não encontrado.");
            product.UpdateStock(new Quantity(quantity));
            await _productRepository.UpdateAsync(product);
            await _productRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductDTO>> GetByCategoryIdAsync(int categoryId)
        {
            var products = await _productRepository.GetByCategoryIdAsync(categoryId);
            return Mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public override async Task<ProductDTO> AddAsync(ProductDTO dto)
        {
            var product = new Product(new SKU(dto.Sku), dto.Name, dto.Description,
                new Money(dto.Price), new Quantity(dto.StockQuantity));
            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();
            return Mapper.Map<ProductDTO>(product);
        }
    }
}