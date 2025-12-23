using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.Common;

namespace DataAccessLayer.Repositories.Interface.RoleAdmin
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<List<Role>> GetAllExcludeAdminAsync();
        Task<bool> RoleExistsAsync(int roleId);

        IQueryable<Role> GetQueryable();
    }
}
