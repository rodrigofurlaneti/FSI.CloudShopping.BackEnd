namespace FSI.CloudShopping.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Infrastructure.Data;

public class CouponRepository : Repository<Coupon, Guid>, ICouponRepository
{
    public CouponRepository(AppDbContext context) : base(context) { }

    public async Task<Coupon?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(c =>
                c.Code.ToLower() == code.ToLower() && c.IsActive,
                cancellationToken);
    }

    public async Task<(IEnumerable<Coupon>, int)> GetPagedAsync(
        int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsNoTracking();
        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return (items, total);
    }

    public async Task<IEnumerable<Coupon>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(c => c.IsActive && c.ValidTo >= DateTime.UtcNow)
            .OrderBy(c => c.Code)
            .ToListAsync(cancellationToken);
    }
}
