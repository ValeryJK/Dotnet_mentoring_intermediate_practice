using EventBookSystem.DAL.Entities.Interfaces;
using System.Linq.Expressions;

namespace EventBookSystem.DAL.Repositories
{
    public interface IRepositoryBase<TEntity> where TEntity : IEntity
    {
        IQueryable<TEntity> FindAll(bool trackChanges);
        IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression,
        bool trackChanges);
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
