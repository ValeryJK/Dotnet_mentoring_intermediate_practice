using EventBookSystem.DAL.Entities.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace EventBookSystem.DAL.Repositories
{
    public interface IRepositoryBase<TEntity> where TEntity : IEntity
    {
        Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken);

        IQueryable<TEntity> GetAll(bool trackChanges);

        void Create(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        Task SaveAsync();
    }
}