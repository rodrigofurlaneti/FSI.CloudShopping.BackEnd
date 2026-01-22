using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Application.Mappings;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Interfaces.Services;

namespace FSI.CloudShopping.Application.Services
{
    public class OrderAppService : BaseAppService<OrderDTO, Order>, IOrderAppService
    {
        private readonly IOrderService _domainOrderService;
        private readonly IOrderRepository _orderRepository;

        public OrderAppService(
            IOrderRepository repository,
            IOrderService domainOrderService) : base(repository)
        {
            _domainOrderService = domainOrderService;
            _orderRepository = repository;
        }

        public async Task<OrderDTO> PlaceOrderAsync(int cartId)
        {
            var order = await _domainOrderService.PlaceOrderAsync(cartId);
            await _orderRepository.SaveChangesAsync();
            return order.ToDto();
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByCustomerIdAsync(int customerId)
        {
            var orders = await _orderRepository.GetOrdersByCustomerIdAsync(customerId);
            return orders.Select(o => o.ToDto());
        }

        protected override Order MapToEntity(OrderDTO dto) => dto.ToEntity();
        protected override OrderDTO MapToDto(Order entity) => entity.ToDto();
    }
}