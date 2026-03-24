namespace FSI.CloudShopping.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;
using FSI.CloudShopping.Infrastructure.Data;

public class CustomerRepository : Repository<Customer, Guid>, ICustomerRepository
{
    public CustomerRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Customer?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(c => c.Individual)
            .Include(c => c.Company)
            .Include(c => c.Addresses)
            .Include(c => c.Contacts)
            .FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
    }

    public async Task<Customer?> GetBySessionTokenAsync(Guid sessionToken, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(c => c.Individual)
            .Include(c => c.Company)
            .FirstOrDefaultAsync(c => c.SessionToken == sessionToken, cancellationToken);
    }

    public async Task<Customer?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(c => c.RefreshToken == refreshToken, cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(c => c.Email == email, cancellationToken);
    }
}
