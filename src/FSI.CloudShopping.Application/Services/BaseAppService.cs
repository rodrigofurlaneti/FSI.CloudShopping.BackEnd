using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Interfaces;

namespace FSI.CloudShopping.Application.Services;

public abstract class BaseAppService<TDTO, TEntity> : IBaseAppService<TDTO>
    where TDTO : class
    where TEntity : Entity
{
    protected readonly IRepository<TEntity> Repository;

    protected BaseAppService(IRepository<TEntity> repository)
    {
        Repository = repository;
    }

    protected abstract TEntity MapToEntity(TDTO dto);
    protected abstract TDTO MapToDto(TEntity entity);

    public virtual async Task<TDTO> AddAsync(TDTO dto)
    {
        var entity = MapToEntity(dto);
        await Repository.AddAsync(entity);
        await Repository.SaveChangesAsync();
        return MapToDto(entity);
    }

    public virtual async Task<TDTO?> GetByIdAsync(int id)
    {
        var entity = await Repository.GetByIdAsync(id);
        return entity == null ? null : MapToDto(entity);
    }

    public virtual async Task<IEnumerable<TDTO>> GetAllAsync()
    {
        var entities = await Repository.GetAllAsync();
        return entities.Select(MapToDto);
    }

    public virtual async Task UpdateAsync(TDTO dto)
    {
        var entity = MapToEntity(dto);
        await Repository.UpdateAsync(entity);
        await Repository.SaveChangesAsync();
    }

    public virtual async Task RemoveAsync(int id)
    {
        await Repository.RemoveAsync(id);
        await Repository.SaveChangesAsync();
    }

    public void Dispose()
    {
        Repository.Dispose();
        GC.SuppressFinalize(this);
    }
}