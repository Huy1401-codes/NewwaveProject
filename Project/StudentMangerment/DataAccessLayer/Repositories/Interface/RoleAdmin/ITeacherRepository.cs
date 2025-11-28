using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interface.RoleAdmin
{
    public interface ITeacherRepository
    {
        Task<Teacher> GetByIdAsync(int id);
        Task<IEnumerable<Teacher>> GetAllAsync();
        Task<(IEnumerable<Teacher> Data, int TotalCount)> GetPagedAsync(int page, int pageSize, string search);
        Task AddAsync(Teacher teacher);
        Task UpdateAsync(Teacher teacher);
        Task SoftDeleteAsync(int id);
        Task SaveAsync();

        Task<IEnumerable<Teacher>> GetAllNameAsync();

    }
}
