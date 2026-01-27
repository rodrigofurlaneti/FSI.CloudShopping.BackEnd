using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace FSI.CloudShopping.Infrastructure.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : Entity
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

        // 👈 Aqui retornamos o ID gerado
        public virtual async Task<int> AddAsync(T entity)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcInsert);
            SetInsertParameters(cmd, entity);
            var result = await cmd.ExecuteScalarAsync();
            int generatedId = Convert.ToInt32(result);
            entity.Id = generatedId; // ✅ Aqui funciona por causa do 'protected set'
            return generatedId;
        }

        public abstract Task UpdateAsync(T entity);
        public virtual async Task<T?> GetByIdAsync(int id)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcGetById);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapToEntity(reader) : null;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var list = new List<T>();
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcGetAll);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                list.Add(MapToEntity(reader));
            return list;
        }

        public virtual async Task RemoveAsync(int id)
        {
            using var cmd = await Connector.CreateProcedureCommandAsync(ProcDelete);
            cmd.Parameters.AddWithValue("@Id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        public virtual async Task<int> SaveChangesAsync() => await Task.FromResult(1);

        protected abstract void SetInsertParameters(SqlCommand cmd, T entity);
        protected abstract T MapToEntity(SqlDataReader reader);

        public void Dispose()
        {
            Connector?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
