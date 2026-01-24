using FSI.CloudShopping.Domain.Entities;
namespace FSI.CloudShopping.Domain.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<Payment?> GetByOrderIdAsync(int orderId);
        Task<IEnumerable<Payment>> GetHistoryByOrderIdAsync(int orderId);
    }
}
