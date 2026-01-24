using FSI.CloudShopping.Application.DTOs.Order;
using FSI.CloudShopping.Application.Interfaces;
namespace FSI.CloudShopping.Application.Interfaces
{
    public interface IOrderAppService : IBaseAppService<OrderDTO>
    {
        Task<OrderDTO> PlaceOrderAsync(CheckoutRequest request);
        Task<IEnumerable<OrderDTO>> GetCustomerHistoryAsync(int customerId);
    }
}