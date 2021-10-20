using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using pdouelle.Entity;

namespace pdouelle.Blueprints.Repositories
{
    public interface IRepository<TEntity> 
        where TEntity : IEntity
    {
        IQueryable<TEntity> GetAll();
        Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken);
        Task AddRangeAsync(List<TEntity> entities, CancellationToken cancellationToken);
        void Update(TEntity entity);
        void UpdateRange(List<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(List<TEntity> entities);
        Task<bool> SaveAsync(CancellationToken cancellationToken);
    }
}