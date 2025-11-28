using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interface.RoleAdmin
{
    public interface IClassStudentRepository
    {
        Task AddStudentToClassAsync(int classId, int studentId);
        Task RemoveStudentFromClassAsync(int classId, int studentId);
        Task<List<ClassStudent>> GetStudentsByClassIdAsync(int classId);
        Task SaveAsync();
    }

}
