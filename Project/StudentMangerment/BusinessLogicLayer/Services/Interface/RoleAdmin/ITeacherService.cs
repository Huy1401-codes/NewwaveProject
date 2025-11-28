using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Interface.RoleAdmin
{
    public interface ITeacherService
    {
        Task<Teacher> GetByIdAsync(int id);
        Task<(IEnumerable<Teacher> Data, int TotalCount)> GetPagedAsync(int page, int pageSize, string search);
        Task<bool> CreateAsync(Teacher teacher);
        Task<bool> UpdateAsync(Teacher teacher);
        Task<bool> SoftDeleteAsync(int id);

        Task<IEnumerable<Teacher>> GetAllAsync();

    }

}
