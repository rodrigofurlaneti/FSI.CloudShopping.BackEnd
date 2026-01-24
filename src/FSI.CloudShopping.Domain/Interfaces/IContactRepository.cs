using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
namespace FSI.CloudShopping.Domain.Interfaces
{
    public interface IContactRepository : IRepository<Contact>
    {
        Task<IEnumerable<Contact>> GetByCustomerIdAsync(int customerId);
    }
}