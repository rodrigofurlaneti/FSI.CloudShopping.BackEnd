using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Interfaces.Services;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Domain.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task MergeCartAsync(VisitorToken token, int customerId)
        {
            var visitorCart = await _cartRepository.GetByVisitorTokenAsync(token);
            if (visitorCart is null) return;
            var customerCart = await GetOrCreateCustomerCart(customerId);
            TransferItems(visitorCart, customerCart);
            await _cartRepository.UpdateAsync(customerCart);
            await _cartRepository.RemoveAsync(visitorCart.Id);
        }
        private async Task<Cart> GetOrCreateCustomerCart(int customerId)
        {
            var cart = await _cartRepository.GetByCustomerIdAsync(customerId);
            return cart ?? new Cart(customerId);
        }
        private void TransferItems(Cart source, Cart destination)
        {
            foreach (var item in source.Items)
            {
                destination.AddOrUpdateItem(item.ProductId, item.Quantity, item.UnitPrice);
            }
        }
    }
}