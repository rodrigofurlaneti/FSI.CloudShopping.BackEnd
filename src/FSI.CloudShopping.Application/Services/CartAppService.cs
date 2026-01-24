using AutoMapper;
using FSI.CloudShopping.Application.DTOs.Cart;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;

namespace FSI.CloudShopping.Application.Services
{
    public class CartAppService : BaseAppService<Cart, CartDTO>, ICartAppService
    {
        private readonly ICartRepository _cartRepository;

        public CartAppService(ICartRepository cartRepository, IMapper mapper)
            : base(cartRepository, mapper)
        {
            _cartRepository = cartRepository;
        }

        public async Task MergeAfterLoginAsync(Guid visitorToken, int customerId)
        {
            var visitorCart = await _cartRepository.GetBySessionTokenAsync(visitorToken);
            var userCart = await _cartRepository.GetByCustomerIdAsync(customerId);
            if (visitorCart == null) return;
            if (userCart == null) userCart = new Cart(customerId);

            foreach (var item in visitorCart.Items)
                userCart.AddOrUpdateItem(item.ProductId, item.Quantity, item.UnitPrice);

            await _cartRepository.UpdateAsync(userCart);
            await _cartRepository.RemoveAsync(visitorCart.Id);
            await _cartRepository.SaveChangesAsync();
        }

        public async Task ClearCartAsync(string token)
        {
            if (!Guid.TryParse(token, out Guid sessionToken)) return;
            var cart = await _cartRepository.GetBySessionTokenAsync(sessionToken);
            if (cart == null) return;
            await _cartRepository.RemoveAsync(cart.Id);
            await _cartRepository.SaveChangesAsync();
        }
    }
}