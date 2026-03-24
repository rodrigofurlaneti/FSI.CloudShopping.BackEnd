namespace FSI.CloudShopping.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Infrastructure.Data;

public class AuditLogRepository : Repository<AuditLog, Guid>, IAuditLogRepository
{
    public AuditLogRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<AuditLog>> GetByEntityAsync(
        string entityName, string entityId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(a => a.EntityName == entityName && a.EntityId == entityId)
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<AuditLog>, int)> GetByUserAsync(
        string userId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsNoTracking().Where(a => a.UserId == userId);
        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(a => a.Timestamp)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return (items, total);
    }

    public async Task<(IEnumerable<AuditLog>, int)> GetByDateRangeAsync(
        DateTime from, DateTime to, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsNoTracking()
            .Where(a => a.Timestamp >= from && a.Timestamp <= to);
        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(a => a.Timestamp)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return (items, total);
    }
}
