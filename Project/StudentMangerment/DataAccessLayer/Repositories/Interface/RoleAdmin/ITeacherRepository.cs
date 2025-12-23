using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.Common;

namespace DataAccessLayer.Repositories.Interface.RoleAdmin
{
    public interface ITeacherRepository : IRepository<Teacher>
    {
        Task<(IEnumerable<Teacher> Data, int TotalCount)>
            GetPagedAsync(int page, int pageSize, string search);

        Task SoftDeleteAsync(int id);

        Task<IEnumerable<Teacher>> GetAllNameAsync();

        Task<IEnumerable<User>> GetUsersByRoleAsync(string role);

        Task<int?> GetTeacherIdByUserIdAsync(int userId);
    }
}
