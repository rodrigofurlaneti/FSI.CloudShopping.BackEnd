using FSI.CloudShopping.Domain.Entities;
namespace FSI.CloudShopping.Domain.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart?> GetByVisitorTokenAsync(Guid token);
        Task<Cart?> GetByCustomerIdAsync(int customerId);
        Task<IEnumerable<Cart>> GetExpiredCartsAsync(DateTime expirationDate);
    }
}