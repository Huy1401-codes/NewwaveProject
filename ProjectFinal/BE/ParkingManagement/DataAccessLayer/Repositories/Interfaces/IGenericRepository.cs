using System.Linq.Expressions;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(int id);
        Task<IReadOnlyList<TEntity>> GetAllAsync();

        Task<IReadOnlyList<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> predicate);

        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
       

    }
}
