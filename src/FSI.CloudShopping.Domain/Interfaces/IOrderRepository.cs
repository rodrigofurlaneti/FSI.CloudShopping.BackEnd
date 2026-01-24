using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Domain.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId);
        Task<Order?> GetOrderWithItemsAsync(int orderId);
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);
    }
}
