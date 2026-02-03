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
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CartAppService(ICartRepository cartRepository, IProductRepository productRepository,
            ICustomerRepository customerRepository, IMapper mapper)
            : base(cartRepository, mapper)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _mapper = mapper;
        }
        public async Task<CartDTO> AddItemAsync(Guid sessionToken, int productId, int quantity)
        {
            var customerId = await _customerRepository.GetIdBySessionTokenAsync(sessionToken);
            if (customerId == 0)
                throw new DomainException("Sessão expirada ou inválida.");
            var cart = await _cartRepository.GetByCustomerIdAsync(customerId) ?? new Cart(customerId);
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) throw new DomainException("Produto não encontrado.");
            cart.AddOrUpdateItem(productId, new Quantity(quantity), new Money(product.Price.Value));
            if (cart.Id == 0) await _cartRepository.AddAsync(cart);
            else await _cartRepository.UpdateAsync(cart);
            await _cartRepository.SaveChangesAsync();
            return _mapper.Map<CartDTO>(cart); 
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