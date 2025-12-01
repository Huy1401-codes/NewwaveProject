using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Teacher;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;

namespace BusinessLogicLayer.Services.Interface.RoleTeacher
{
    public interface ITeacherService
    {
        Task<(IEnumerable<Class>, int)> GetTeacherClassesAsync(
            int teacherId, int page, int pageSize, string search, int? semesterId);

        Task<(IEnumerable<StudentDto>, int)> GetStudentsInClassAsync(int classId, int page, int size, string search);

        Task<bool> UpdateGradeAsync(int classId, int studentId, Dictionary<int, double> scores);
        Task<byte[]> ExportGradesAsync(int classId);

        Task<bool> ImportGradesAsync(int classId, IFormFile file);

        Task<ClassStatisticsDto> GetClassStatisticsAsync(int classId);


        Task<StudentDto> GetStudentByIdAsync(int studentId);
        Task<IEnumerable<GradeComponentDto>> GetGradeComponentsByClassIdAsync(int classId, int studentId);

    }
}
