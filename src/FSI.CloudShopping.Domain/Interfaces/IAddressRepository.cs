namespace FSI.CloudShopping.Domain.Interfaces;

using FSI.CloudShopping.Domain.Entities;

public interface IAddressRepository : IRepository<Address, Guid>
{
    Task<IEnumerable<Address>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<Address?> GetDefaultByCustomerAsync(Guid customerId, CancellationToken cancellationToken = default);
}
