using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interface.RoleAdmin
{
    public interface IClassStudentRepository : IRepository<ClassStudent>
    {
        Task AddStudentToClassAsync(int classId, int studentId);
        Task RemoveStudentFromClassAsync(int classId, int studentId);
        Task<List<ClassStudent>> GetStudentsByClassIdAsync(int classId);
    }

}
