namespace FSI.CloudShopping.Application.Services;

using AutoMapper;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Interfaces;

/// <summary>
/// Legacy base application service retained for backward compatibility with non-CQRS flows.
/// New features should prefer dedicated CQRS Command/Query handlers via MediatR.
/// Updated to use Entity&lt;TId&gt; and IRepository&lt;TEntity, TId&gt; generics.
/// </summary>
public abstract class BaseAppService<TEntity, TId, TDTO> : IBaseAppService<TDTO>
    where TEntity : Entity<TId>
    where TId : notnull
    where TDTO : class
{
    protected readonly IRepository<TEntity, TId> Repository;
    protected readonly IMapper Mapper;

    protected BaseAppService(IRepository<TEntity, TId> repository, IMapper mapper)
    {
        Repository = repository;
        Mapper = mapper;
    }

    public virtual async Task<TDTO> AddAsync(TDTO dto)
    {
        var entity = Mapper.Map<TEntity>(dto);
        await Repository.AddAsync(entity);
        await Repository.SaveChangesAsync();
        return Mapper.Map<TDTO>(entity);
    }

    public virtual async Task<TDTO?> GetByIdAsync(TId id)
    {
        var entity = await Repository.GetByIdAsync(id);
        return entity is null ? null : Mapper.Map<TDTO>(entity);
    }

    public virtual async Task<IEnumerable<TDTO>> GetAllAsync()
    {
        var entities = await Repository.GetAllAsync();
        return Mapper.Map<IEnumerable<TDTO>>(entities);
    }

    public virtual async Task UpdateAsync(TDTO dto)
    {
        var entity = Mapper.Map<TEntity>(dto);
        await Repository.UpdateAsync(entity);
        await Repository.SaveChangesAsync();
    }
}
