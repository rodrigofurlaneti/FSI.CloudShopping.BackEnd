using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Interfaces.Services;
namespace FSI.CloudShopping.Domain.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IProductRepository _productRepository;

        public InventoryService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task ValidateAndReserveStockAsync(IEnumerable<CartItem> items)
        {
            foreach (var item in items)
            {
                await DebitProductStock(item);
            }
        }

        private async Task DebitProductStock(CartItem item)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);

            if (product is null)
                throw new DomainException($"Produto {item.ProductId} não localizado no catálogo.");

            product.DebitStock(item.Quantity);

            await _productRepository.UpdateAsync(product);
        }
    }
}