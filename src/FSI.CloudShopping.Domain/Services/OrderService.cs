using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Interfaces.Services;
namespace FSI.CloudShopping.Domain.Services
{
    public class OrderService : IOrderService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IInventoryService _inventoryService;

        public OrderService(
            ICartRepository cartRepository,
            IOrderRepository orderRepository,
            IInventoryService inventoryService)
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _inventoryService = inventoryService;
        }

        public async Task<Order> PlaceOrderAsync(int cartId)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId)
                       ?? throw new DomainException("Carrinho não encontrado para checkout.");
            await _inventoryService.ValidateAndReserveStockAsync(cart.Items);
            var order = CreateOrderFromCart(cart);
            await _orderRepository.AddAsync(order);
            await _cartRepository.RemoveAsync(cart.Id);
            return order;
        }

        private Order CreateOrderFromCart(Cart cart)
        {
            if (cart.CustomerId is null)
                throw new DomainException("Apenas clientes cadastrados podem finalizar pedidos.");
            return new Order(cart.CustomerId.Value, cart.Items);
        }
    }
}
