using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Domain.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart?> GetByCustomerIdAsync(int customerId);
        Task<Cart?> GetBySessionTokenAsync(Guid token);
        Task<Cart?> GetByEmailAsync(Email email);
        Task UpdateItemsAsync(Cart cart);
    }
}