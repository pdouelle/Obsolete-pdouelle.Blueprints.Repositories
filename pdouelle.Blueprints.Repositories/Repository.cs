using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using pdouelle.Entity;

namespace pdouelle.Blueprints.Repositories
{
    public class Repository<TEntity, TDbContext> : IRepository<TEntity>
        where TEntity : class, IEntity
        where TDbContext : DbContext
    {
        private readonly TDbContext _context;

        public Repository(TDbContext context)
        {
            Guard.Against.Null(context, nameof(context));

            _context = context;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }

        public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
            await _context.Set<TEntity>().FindAsync(new object[]{id}, cancellationToken);

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            Guard.Against.Null(entity, nameof(entity));

            await _context.AddAsync(entity, cancellationToken);
        }
        
        public async Task AddRangeAsync(List<TEntity> entities, CancellationToken cancellationToken)
        {
            Guard.Against.NullOrEmpty(entities, nameof(entities));

            await _context.AddRangeAsync(entities, cancellationToken);
        }

        public void Update(TEntity entity)
        {
            Guard.Against.Null(entity, nameof(entity));
            
            _context.Update(entity);
        }
        
        public void UpdateRange(List<TEntity> entities)
        {
            Guard.Against.NullOrEmpty(entities, nameof(entities));
            
            _context.UpdateRange(entities);
        }

        public void Remove(TEntity entity)
        {
            Guard.Against.Null(entity, nameof(entity));

            _context.Remove(entity);
        }
        
        public void RemoveRange(List<TEntity> entities)
        {
            Guard.Against.NullOrEmpty(entities, nameof(entities));

            _context.Set<TEntity>().RemoveRange(entities);
        }
        
        public async Task<bool> SaveAsync(CancellationToken cancellationToken) =>
            await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}