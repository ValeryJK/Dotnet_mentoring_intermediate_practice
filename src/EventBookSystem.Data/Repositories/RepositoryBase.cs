using EventBookSystem.DAL.DataContext;
using EventBookSystem.DAL.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace EventBookSystem.DAL.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class, IEntity
    {
        protected MainDBContext _context;

        protected RepositoryBase(MainDBContext context)
        {
            _context = context;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken)
        {
            return await _context.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
        }

        public IQueryable<TEntity> GetAll(bool trackChanges) =>
            !trackChanges ? _context.Set<TEntity>().AsNoTracking() : _context.Set<TEntity>();

        public void Create(TEntity entity) => _context.Set<TEntity>().Add(entity);

        public void Update(TEntity entity) => _context.Set<TEntity>().Update(entity);

        public void Delete(TEntity entity) => _context.Set<TEntity>().Remove(entity);

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}