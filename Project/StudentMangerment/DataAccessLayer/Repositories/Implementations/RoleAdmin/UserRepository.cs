using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Implementations.Common;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccessLayer.Repositories.Implementations.RoleAdmin
{
    public class UserRepository
     : Repository<User>, IUserRepository
    {
        public UserRepository(SchoolContext context)
            : base(context) { }

        public IQueryable<User> GetAllQueryable()
        {
            return _dbSet
                .Where(u => u.IsStatus == true)
                .Where(u => u.UserRoles.Any(ur => ur.Role.Name != "Admin"))
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role);
        }

        public IQueryable<User> GetAllRestoreQueryable()
        {
            return _dbSet
                .Where(u => u.IsStatus == true)
                .Where(u => u.UserRoles.Any(ur => ur.Role.Name != "Admin"))
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u =>
                    u.Username == username &&
                    u.IsStatus == true);
        }

        public async Task<User?> FirstOrDefaultAsync(
            Expression<Func<User, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task SoftDeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                user.IsStatus = false;
                _dbSet.Update(user);
            }
        }
    }


}
