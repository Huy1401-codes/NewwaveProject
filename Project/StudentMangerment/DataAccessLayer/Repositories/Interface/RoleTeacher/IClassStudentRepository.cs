using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interface.RoleTeacher
{
    public interface IClassStudentRepository
    {
        Task<(IEnumerable<Student> Data, int TotalCount)>
            GetStudentsByClassAsync(int classId, int page, int pageSize, string search);

        Task<List<Student>> GetStudentsWithUserByClassAsync(int classId);

        Task<Student> GetStudentByIdAsync(int studentId);

    }

}
