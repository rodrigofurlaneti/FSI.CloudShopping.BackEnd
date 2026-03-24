namespace FSI.CloudShopping.Infrastructure.Repositories;

using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Infrastructure.Data;
using Microsoft.Data.SqlClient;

/// <summary>
/// Legacy base repository using stored procedures for entities with int ID.
/// Kept for backward compatibility with Authentication stored procedure flow.
/// New repositories should extend Repository&lt;TEntity, TId&gt; (EF Core).
/// </summary>
public abstract class BaseRepository<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : notnull
{
    protected readonly SqlDbConnector Connector;

    protected abstract string ProcInsert { get; }
    protected abstract string ProcUpdate { get; }
    protected abstract string ProcDelete { get; }
    protected abstract string ProcGetById { get; }
    protected abstract string ProcGetAll { get; }

    protected BaseRepository(SqlDbConnector connector)
    {
        Connector = connector;
    }

    public virtual async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        using var cmd = await Connector.CreateProcedureCommandAsync(ProcGetById);
        cmd.Parameters.AddWithValue("@Id", id);
        using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        return await reader.ReadAsync(cancellationToken) ? MapToEntity(reader) : null;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = new List<TEntity>();
        using var cmd = await Connector.CreateProcedureCommandAsync(ProcGetAll);
        using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
            list.Add(MapToEntity(reader));
        return list;
    }

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        using var cmd = await Connector.CreateProcedureCommandAsync(ProcInsert);
        SetInsertParameters(cmd, entity);
        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    public abstract Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        using var cmd = await Connector.CreateProcedureCommandAsync(ProcDelete);
        cmd.Parameters.AddWithValue("@Id", entity.Id);
        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    public virtual Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    protected abstract void SetInsertParameters(SqlCommand cmd, TEntity entity);
    protected abstract TEntity MapToEntity(SqlDataReader reader);

    public void Dispose()
    {
        Connector?.Dispose();
        GC.SuppressFinalize(this);
    }
}
