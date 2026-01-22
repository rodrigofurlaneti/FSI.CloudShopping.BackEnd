using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Application.Mappings;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Interfaces.Services;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Application.Services
{
    public class CartAppService : ICartAppService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartService _domainCartService;
        public CartAppService(
            ICartRepository cartRepository,
            IProductRepository productRepository,
            ICartService domainCartService)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _domainCartService = domainCartService;
        }
        public async Task AddItemAsync(string token, AddItemDTO dto)
        {
            var product = await _productRepository.GetByIdAsync(dto.ProductId);
            if (product == null)
                throw new KeyNotFoundException("Produto não encontrado.");
            var visitorToken = new VisitorToken(Guid.Parse(token));
            var cart = await _cartRepository.GetByVisitorTokenAsync(visitorToken)
                       ?? new Cart(visitorToken);
            cart.AddOrUpdateItem(product.Id, new Quantity(dto.Quantity), product.Price);
            if (cart.Id == 0)
                await _cartRepository.AddAsync(cart);
            else
                await _cartRepository.UpdateAsync(cart);
            await _cartRepository.SaveChangesAsync();
        }
        public async Task MergeAfterLoginAsync(Guid visitorToken, int customerId)
        {
            await _domainCartService.MergeCartAsync(new VisitorToken(visitorToken), customerId);
            await _cartRepository.SaveChangesAsync();
        }
        public async Task<CartDTO> GetCartAsync(string token)
        {
            var visitorToken = new VisitorToken(Guid.Parse(token));
            var cart = await _cartRepository.GetByVisitorTokenAsync(visitorToken);
            return cart.ToDto(); 
        }
        public async Task ClearCartAsync(string token)
        {
            var visitorToken = new VisitorToken(Guid.Parse(token));
            var cart = await _cartRepository.GetByVisitorTokenAsync(visitorToken);
            if (cart == null) return;
            await _cartRepository.RemoveAsync(cart.Id);
            await _cartRepository.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(string token, int productId)
        {
            var cart = await _cartRepository.GetByVisitorTokenAsync(new VisitorToken(Guid.Parse(token)));
            if (cart == null) return;
            cart.RemoveItem(productId);
            await _cartRepository.UpdateAsync(cart);
            await _cartRepository.SaveChangesAsync();
        }
    }
}