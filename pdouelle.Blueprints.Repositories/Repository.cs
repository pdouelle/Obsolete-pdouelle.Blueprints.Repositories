using System;

namespace pdouelle.Blueprints.Repositories
{
    public class Repository<TEntity, TDbContext> : IRepository<TEntity>
        where TEntity : class, IEntity
        where TDbContext : DbContext
    {
        private readonly TDbContext _context;

        public Repository(TDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveAsync(CancellationToken cancellationToken = new()) =>
            await _context.SaveChangesAsync(cancellationToken) > 0;

        public void Create(TEntity entity) =>
            _context.Set<TEntity>().AddAsync(entity);

        public void Edit(TEntity entity) =>
            _context.Set<TEntity>().Update(entity);

        public void Delete(TEntity entity) =>
            _context.Set<TEntity>().Remove(entity);

        public async Task<TEntity> GetByIdAsync(Guid id) =>
            await _context.Set<TEntity>().FindAsync(id);

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = new()) =>
            await _context.Set<TEntity>().ToListAsync(cancellationToken);

        public IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (include != null)
                query = include(query);

            if (predicate != null)
                query = query.Where(predicate);

            return query;
        }
    }
}