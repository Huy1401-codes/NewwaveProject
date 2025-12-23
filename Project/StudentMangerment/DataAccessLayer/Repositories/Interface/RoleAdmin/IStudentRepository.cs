using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.Common;

namespace DataAccessLayer.Repositories.Interface.RoleAdmin
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<(IEnumerable<Student> Data, int TotalCount)>
            GetPagedAsync(int page, int pageSize, string search);

        Task<IEnumerable<Student>> GetAllNameAsync();
        Task<Student?> GetByStudentCodeAsync(string studentCode);
    
        Task<IEnumerable<User>> GetUsersByRoleAsync(string role);
    }
}
