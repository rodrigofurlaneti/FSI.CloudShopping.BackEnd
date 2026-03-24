using AutoMapper;
using FSI.CloudShopping.Application.DTOs.Order;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;

namespace FSI.CloudShopping.Application.Services
{
    public class OrderAppService : BaseAppService<Order, int, OrderDTO>, IOrderAppService
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
            var subTotal = new Domain.ValueObjects.Money(cart.Items.Sum(i => i.UnitPrice.Amount * i.Quantity));
            var zero = new Domain.ValueObjects.Money(0);
            var order = Order.Create(request.CustomerId, request.ShippingAddressId, subTotal, zero, zero);
            await _orderRepository.AddAsync(order);
            await _cartRepository.RemoveAsync(cart.Id);
            await _orderRepository.SaveChangesAsync();
            return Mapper.Map<OrderDTO>(order);
        }
        public async Task<IEnumerable<OrderDTO>> GetCustomerHistoryAsync(Guid customerId)
        {
            var orders = await _orderRepository.GetByCustomerIdAsync(customerId);
            return Mapper.Map<IEnumerable<OrderDTO>>(orders);
        }
        public override Task<OrderDTO> AddAsync(OrderDTO dto)
        {
            throw new DomainException("Pedidos devem ser criados via PlaceOrderAsync.");
        }
    }
}