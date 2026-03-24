namespace FSI.CloudShopping.Domain.Core;

/// <summary>
/// Interface for the Unit of Work pattern. Coordinates the work of multiple repositories
/// and provides a single commit point.
/// </summary>
public interface IUnitOfWork : IAsyncDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
