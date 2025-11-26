using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interface.RoleTeacher
{
    public interface IGradeRepository
    {
        Task<List<StudentGrade>> GetStudentGradesAsync(int studentId, int classId);
        Task<StudentGrade?> GetSingleGradeAsync(int studentId, int classId, int componentId);
        Task<bool> AddOrUpdateGradeAsync(StudentGrade grade);
        Task<List<StudentGrade>> GetGradesByClassAsync(int classId);
        Task SaveAsync();

        Task<List<GradeComponent>> GetGradeComponentsAsync(int subjectId);

    }


}
