namespace FSI.CloudShopping.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Infrastructure.Data;

public class AddressRepository : Repository<Address, Guid>, IAddressRepository
{
    public AddressRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Address>> GetByCustomerIdAsync(
        Guid customerId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(a => a.CustomerId == customerId)
            .OrderBy(a => a.AddressType)
            .ThenByDescending(a => a.IsDefault)
            .ToListAsync(cancellationToken);
    }
}
