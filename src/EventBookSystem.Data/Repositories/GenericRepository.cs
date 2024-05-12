using EventBookSystem.DAL.DataContext;
using EventBookSystem.DAL.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventBookSystem.DAL.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class, IEntity
    {
        protected MainDBContext _context;

        public RepositoryBase(MainDBContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> FindAll(bool trackChanges) =>
            !trackChanges ? _context.Set<TEntity>().AsNoTracking() : _context.Set<TEntity>();

        public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges) =>
            !trackChanges ? _context.Set<TEntity>().Where(expression).AsNoTracking() : _context.Set<TEntity>().Where(expression);

        public void Create(TEntity entity) => _context.Set<TEntity>().Add(entity);
        public void Update(TEntity entity) => _context.Set<TEntity>().Update(entity);
        public void Delete(TEntity entity) => _context.Set<TEntity>().Remove(entity);

    }
}
