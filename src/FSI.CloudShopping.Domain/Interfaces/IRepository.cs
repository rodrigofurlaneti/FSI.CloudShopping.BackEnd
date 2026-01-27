using FSI.CloudShopping.Domain.Core;
namespace FSI.CloudShopping.Domain.Interfaces
{
    public interface IRepository<T> : IDisposable where T : Entity
    {
        Task<int> AddAsync(T entity);
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task UpdateAsync(T entity);
        Task RemoveAsync(int id);
        Task<int> SaveChangesAsync();
    }
}
