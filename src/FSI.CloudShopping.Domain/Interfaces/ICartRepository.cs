using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Domain.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart?> GetByVisitorTokenAsync(VisitorToken token);
        Task<Cart?> GetByCustomerIdAsync(int customerId);
        Task<IEnumerable<Cart>> GetExpiredCartsAsync(DateTime expirationDate);
    }
}