using AutoMapper;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Interfaces;

namespace FSI.CloudShopping.Application.Services
{
    public abstract class BaseAppService<TEntity, TDTO> : IBaseAppService<TDTO>
        where TEntity : Entity
        where TDTO : class
    {
        protected readonly IRepository<TEntity> Repository;
        protected readonly IMapper Mapper;

        protected BaseAppService(IRepository<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        public virtual async Task<TDTO> AddAsync(TDTO dto)
        {
            var entity = Mapper.Map<TEntity>(dto);
            var generatedId = await Repository.AddAsync(entity);
            entity.Id = generatedId;
            await Repository.SaveChangesAsync();
            return Mapper.Map<TDTO>(entity);
        }

        public virtual async Task<TDTO?> GetByIdAsync(int id)
        {
            var entity = await Repository.GetByIdAsync(id);
            return Mapper.Map<TDTO>(entity);
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
}
