using EventBookSystem.DAL.Entities.Interfaces;

namespace EventBookSystem.DAL.Repositories
{
    public interface IRepositoryBase<TEntity> where TEntity : IEntity
    {
        IQueryable<TEntity> GetAll(bool trackChanges);

        void Create(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        Task SaveAsync();
    }
}