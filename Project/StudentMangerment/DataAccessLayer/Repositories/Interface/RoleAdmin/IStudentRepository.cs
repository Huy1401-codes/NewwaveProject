using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interface.RoleAdmin
{
    public interface IStudentRepository
    {
        Task<Student> GetByIdAsync(int id);
        Task<IEnumerable<Student>> GetAllAsync();
        Task<(IEnumerable<Student> Data, int TotalCount)> GetPagedAsync(int page, int pageSize, string search);
        Task AddAsync(Student student);
        Task UpdateAsync(Student student);
        Task SoftDeleteAsync(int id);
        Task SaveAsync();

        Task<IEnumerable<Student>> GetAllNameAsync();

    }
}
