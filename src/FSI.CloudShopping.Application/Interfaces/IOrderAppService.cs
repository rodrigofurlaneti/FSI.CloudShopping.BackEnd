using FSI.CloudShopping.Application.DTOs;

namespace FSI.CloudShopping.Application.Interfaces
{
    public interface IOrderAppService : IBaseAppService<OrderDTO>
    {
        Task<OrderDTO> PlaceOrderAsync(int cartId);
        Task<IEnumerable<OrderDTO>> GetOrdersByCustomerIdAsync(int customerId);
    }
}
