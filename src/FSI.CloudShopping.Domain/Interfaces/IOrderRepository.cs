using FSI.CloudShopping.Domain.Entities;
namespace FSI.CloudShopping.Domain.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId);
        Task<Order?> GetOrderWithItemsAsync(int orderId);
    }
}
