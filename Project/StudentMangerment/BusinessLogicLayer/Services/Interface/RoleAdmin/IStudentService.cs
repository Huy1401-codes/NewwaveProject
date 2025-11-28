using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Interface.RoleAdmin
{
    public interface IStudentService
    {
        Task<Student> GetByIdAsync(int id);
        Task<(IEnumerable<Student> Data, int TotalCount)> GetPagedAsync(int page, int pageSize, string search);
        Task<bool> CreateAsync(Student student);
        Task<bool> UpdateAsync(Student student);
        Task<bool> SoftDeleteAsync(int id);

        Task<IEnumerable<Student>> GetAllAsync();
    }
}
