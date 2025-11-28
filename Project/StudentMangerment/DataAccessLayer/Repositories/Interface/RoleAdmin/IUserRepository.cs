using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interface.RoleAdmin
{
    public interface IUserRepository
    {
        IQueryable<User> GetAllQueryable();

        IQueryable<User> GetAllRestoreQueryable();
      
        Task<User> GetByIdAsync(int id);
        Task<User> GetByUsernameAsync(string username);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task SoftDeleteAsync(int id);
        Task SaveAsync();
        Task<User> FirstOrDefaultAsync(Expression<Func<User, bool>> predicate);
    }


}
