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
    public class RoleRepository : IRoleRepository
    {
        private readonly SchoolContext _context;

        public RoleRepository(SchoolContext context)
        {
            _context = context;
        }

        public IQueryable<Role> GetAllQueryable()
        {
            return _context.Roles.AsQueryable();
        }

        public async Task<List<Role>> GetAllAsync()
        {
            return await _context.Roles.Where(a=>a.RoleName!="Admin").ToListAsync();
        }

        public async Task<Role> GetByIdAsync(int roleId)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleId == roleId);
        }

        public async Task<bool> RoleExistsAsync(int roleId)
        {
            return await _context.Roles
                .AnyAsync(r => r.RoleId == roleId);
        }
    }
}
