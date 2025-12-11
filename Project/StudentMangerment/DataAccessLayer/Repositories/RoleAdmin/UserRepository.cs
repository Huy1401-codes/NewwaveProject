using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.RoleAdmin
{
    public class UserRepository : IUserRepository
    {
        private readonly SchoolContext _context;
        public UserRepository(SchoolContext context) => _context = context;

        public IQueryable<User> GetAllQueryable()
        {
            return _context.Users
                .Where(u => u.IsStatus == true)
                .Where(u => u.UserRoles.Any(ur => ur.Role.Name != "Admin"))
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .AsQueryable();
        }

        public IQueryable<User> GetAllRestoreQueryable()
        {
            return _context.Users
                .Where(u => u.IsStatus == false)
                .Where(u => u.UserRoles.Any(ur => ur.Role.Name != "Admin"))
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .AsQueryable();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
        }


        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Username == username && u.IsStatus == true);
        }

        public async Task AddAsync(User user) => await _context.Users.AddAsync(user);

        public async Task UpdateAsync(User user) => _context.Users.Update(user);

        public async Task SoftDeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsStatus = true;
                _context.Users.Update(user);
            }
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();

        public async Task<User> FirstOrDefaultAsync(Expression<Func<User, bool>> predicate)
        {
            return await _context.Users.FirstOrDefaultAsync(predicate);
        }

    }


}
