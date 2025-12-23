using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.Common;
using System.Linq.Expressions;

namespace DataAccessLayer.Repositories.Interface.RoleAdmin
{
    public interface IUserRepository : IRepository<User>
    {
        IQueryable<User> GetAllQueryable();

        IQueryable<User> GetAllRestoreQueryable();

        Task<User?> GetByUsernameAsync(string username);

        Task<User?> FirstOrDefaultAsync(Expression<Func<User, bool>> predicate);

        Task SoftDeleteAsync(int id);
    }

}
