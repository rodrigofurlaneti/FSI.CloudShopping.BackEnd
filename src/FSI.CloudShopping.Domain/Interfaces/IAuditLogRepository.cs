namespace FSI.CloudShopping.Domain.Interfaces;

using FSI.CloudShopping.Domain.Entities;

public interface IAuditLogRepository : IRepository<AuditLog, Guid>
{
    Task<IEnumerable<AuditLog>> GetByEntityAsync(string entityName, string entityId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditLog>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditLog>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
    Task<(IEnumerable<AuditLog>, int)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}
