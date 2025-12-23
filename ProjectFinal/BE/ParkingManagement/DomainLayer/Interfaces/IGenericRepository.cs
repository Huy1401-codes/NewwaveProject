using DataAccessLayer.Common;
using System.Linq.Expressions;

namespace DomainLayer.Interfaces
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
        Task<PaginatedResult<TEntity>> GetPagedAsync(
                       int pageIndex,
                       int pageSize,
                       Expression<Func<TEntity, bool>>? filter = null,
                       Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                       string includeProperties = "");

    }
}
