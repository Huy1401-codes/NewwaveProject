using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Interface.RoleAdmin
{
    public interface IClassService
    {

        Task<(IEnumerable<Class> data, int totalItems)> GetPagedClassesAsync(
        string? search, int? semesterId, int? subjectId, int? teacherId,
        int page, int pageSize);

        Task<Class?> GetByIdAsync(int id);
        Task<bool> CreateAsync(Class cls);
        Task<bool> UpdateAsync(Class cls);
        Task<bool> DeleteAsync(int id);

        Task<List<Class>> GetAllAsync();
    }
}
