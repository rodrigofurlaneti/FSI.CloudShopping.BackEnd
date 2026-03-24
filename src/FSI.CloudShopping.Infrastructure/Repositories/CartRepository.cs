namespace FSI.CloudShopping.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Infrastructure.Data;

public class CartRepository : Repository<Cart, int>, ICartRepository
{
    public CartRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Cart?> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(c => c.CustomerId == customerId, cancellationToken);
    }

    public async Task<Cart?> GetBySessionTokenAsync(Guid sessionToken, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => Context.Customers
                .Any(cust => cust.SessionToken == sessionToken && cust.Id == c.CustomerId), cancellationToken);
    }

    public async Task DeleteExpiredAsync(CancellationToken cancellationToken = default)
    {
        var expiredCarts = await DbSet
            .Where(c => c.IsExpired)
            .ToListAsync(cancellationToken);

        foreach (var cart in expiredCarts)
        {
            DbSet.Remove(cart);
        }

        await Context.SaveChangesAsync(cancellationToken);
    }
}
