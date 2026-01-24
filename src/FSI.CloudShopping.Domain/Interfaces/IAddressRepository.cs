using FSI.CloudShopping.Domain.Entities;
namespace FSI.CloudShopping.Domain.Interfaces
{
    public interface IAddressRepository : IRepository<Address>
    {
        Task<IEnumerable<Address>> GetByCustomerIdAsync(int customerId);
        Task<Address?> GetDefaultAddressAsync(int customerId);
    }
}
