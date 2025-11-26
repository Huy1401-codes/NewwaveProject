using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories.Interface.RoleAdmin
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAllAsync();
        Task<Role> GetByIdAsync(int roleId);
        Task<bool> RoleExistsAsync(int roleId);

        IQueryable<Role> GetAllQueryable();
    }
}
