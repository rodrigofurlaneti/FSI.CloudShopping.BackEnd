namespace FSI.CloudShopping.Domain.Interfaces;

using FSI.CloudShopping.Domain.Entities;

/// <summary>
/// Repository contract for Contact entity.
/// </summary>
public interface IContactRepository : IRepository<Contact, Guid>
{
    Task<IEnumerable<Contact>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<Contact?> GetPrimaryContactAsync(Guid customerId, CancellationToken cancellationToken = default);
}