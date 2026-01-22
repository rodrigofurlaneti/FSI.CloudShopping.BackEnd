namespace FSI.CloudShopping.Application.Interfaces
{
    public interface IBaseAppService<TDTO> : IDisposable where TDTO : class
    {
        Task<TDTO> AddAsync(TDTO dto);
        Task<TDTO?> GetByIdAsync(int id);
        Task<IEnumerable<TDTO>> GetAllAsync();
        Task UpdateAsync(TDTO dto);
        Task RemoveAsync(int id);
    }
}
