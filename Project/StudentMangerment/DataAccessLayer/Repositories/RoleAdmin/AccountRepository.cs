using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.RoleAdmin
{
    public class AccountRepository : IAccountRepository
    {
        private readonly SchoolContext _context;
        public AccountRepository(SchoolContext context)
        {
            _context = context;
        }

        public async Task<User> GetByUsernameAsync(string email)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
