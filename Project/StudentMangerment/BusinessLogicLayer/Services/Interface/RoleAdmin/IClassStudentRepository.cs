using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Interface.RoleAdmin
{
    public interface IClassStudentRepository
    {
        Task AddStudentToClassAsync(ClassStudent entity);
        Task RemoveStudentFromClassAsync(int classId, int studentId);
        Task<List<ClassStudent>> GetStudentsByClassIdAsync(int classId);
        Task<bool> ExistsAsync(int classId, int studentId);
    }
}
