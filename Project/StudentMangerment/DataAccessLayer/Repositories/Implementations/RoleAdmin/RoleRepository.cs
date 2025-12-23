using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Implementations.Common;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Implementations.RoleAdmin
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(SchoolContext context) : base(context) { }

        public IQueryable<Role> GetQueryable()
            => _dbSet.AsQueryable();

        public async Task<List<Role>> GetAllExcludeAdminAsync()
            => await _dbSet.Where(r => r.Name != "Admin").ToListAsync();

        public async Task<bool> RoleExistsAsync(int roleId)
            => await _dbSet.AnyAsync(r => r.Id == roleId);
    }
}
