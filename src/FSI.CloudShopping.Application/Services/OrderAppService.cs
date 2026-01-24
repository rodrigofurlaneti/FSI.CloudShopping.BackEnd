using AutoMapper;
using FSI.CloudShopping.Application.DTOs.Order;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;

namespace FSI.CloudShopping.Application.Services
{
    public class OrderAppService : BaseAppService<Order, OrderDTO>, IOrderAppService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;

        public OrderAppService(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            IMapper mapper) : base(orderRepository, mapper) 
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
        }
        public async Task<OrderDTO> PlaceOrderAsync(CheckoutRequest request)
        {
            var cart = await _cartRepository.GetByCustomerIdAsync(request.CustomerId);
            if (cart == null || !cart.Items.Any())
                throw new DomainException("Carrinho vazio ou não encontrado.");
            var order = new Order(request.CustomerId, request.ShippingAddressId, cart.Items);
            await _orderRepository.AddAsync(order);
            await _cartRepository.RemoveAsync(cart.Id);
            await _orderRepository.SaveChangesAsync();
            return Mapper.Map<OrderDTO>(order);
        }
        public async Task<IEnumerable<OrderDTO>> GetCustomerHistoryAsync(int customerId)
        {
            var orders = await _orderRepository.GetOrdersByCustomerIdAsync(customerId);
            return Mapper.Map<IEnumerable<OrderDTO>>(orders);
        }
        public override Task<OrderDTO> AddAsync(OrderDTO dto)
        {
            throw new DomainException("Pedidos devem ser criados via PlaceOrderAsync.");
        }
    }
}