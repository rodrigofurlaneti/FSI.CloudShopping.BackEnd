namespace FSI.CloudShopping.Application.Interfaces;

/// <summary>
/// Base contract for legacy application services.
/// New features should use CQRS MediatR handlers instead.
/// </summary>
public interface IBaseAppService<TDTO> where TDTO : class
{
    Task<TDTO> AddAsync(TDTO dto);
    Task<IEnumerable<TDTO>> GetAllAsync();
    Task UpdateAsync(TDTO dto);
}
