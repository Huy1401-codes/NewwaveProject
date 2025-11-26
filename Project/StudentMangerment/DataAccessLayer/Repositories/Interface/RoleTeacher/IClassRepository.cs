using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interface.RoleTeacher
{
    public interface IClassRepository
    {
        Task<(IEnumerable<Class> Data, int TotalCount)>
            GetTeacherClassesAsync(int teacherId, int page, int pageSize, string search, int? semesterId);

        Task<Class> GetByIdAsync(int classId);
    }

}
