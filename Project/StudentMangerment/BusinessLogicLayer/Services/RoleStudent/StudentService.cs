using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.Interface.RoleStudent;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleStudent;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services.RoleStudent
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repo;

        public StudentService(IStudentRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<StudentClassDto>> GetClassesAsync(
    int userId,
    string? search = null,
    DateTime? from = null,
    DateTime? to = null,
    int page = 1,
    int pageSize = 10)
        {
            var studentId = await _repo.GetStudentIdByUserIdAsync(userId);
            if (studentId == null)
                return new List<StudentClassDto>();

            var query = _repo.GetStudentClassesQuery(studentId.Value);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(cs =>
                    cs.Class.ClassName.Contains(search) ||
                    cs.Class.Subject.Name.Contains(search) ||
                    cs.Class.Teacher.User.FullName.Contains(search));
            }

            if (from.HasValue)
                query = query.Where(cs => cs.Class.Semester.StartDate >= from.Value);

            if (to.HasValue)
                query = query.Where(cs => cs.Class.Semester.EndDate <= to.Value);

            var list = await query
                .OrderBy(cs => cs.Class.Semester.StartDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return list.Select(cs => new StudentClassDto
            {
                ClassId = cs.ClassId,
                ClassName = cs.Class.ClassName,
                SubjectName = cs.Class.Subject.Name,
                TeacherName = cs.Class.Teacher.User.FullName,
                StartDate = cs.Class.Semester.StartDate,
                EndDate = cs.Class.Semester.EndDate,
                SemesterName = cs.Class.Semester.Name
            }).ToList();
        }


        /// <summary>
        /// Điểm của học sinh
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public async Task<List<StudentGradeDto>> GetGradesAsync(int studentId)
        {
            var studId = await _repo.GetStudentIdByUserIdAsync(studentId);

            var grades = await _repo.GetStudentGradesQuery((int)studId).ToListAsync();

            var result = grades
                .GroupBy(g => new { g.SubjectId, g.ClassId })
                .Select(g => new StudentGradeDto
                {
                    ClassName = g.First().Class.ClassName,
                    SubjectName = g.First().Subject.Name,
                    Grades = g.Select(x => new GradeDetail
                    {
                        ComponentName = x.GradeComponent.ComponentName,
                        Score = x.Score,
                        Weight = x.GradeComponent.Weight
                    }).ToList(),
                    AverageScore = g.Sum(x => (x.Score ?? 0) * x.GradeComponent.Weight)/ g.Sum(x => x.GradeComponent.Weight)
                }).ToList();

            return result;
        }



        /// <summary>
        /// Danh sách lịch học cho học sinh
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="dayOfWeek"></param>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<StudentScheduleDto>> GetStudentSchedulesAsync(
            int studentId,
            int? dayOfWeek = null,
            string? search = null,
            int page = 1,
            int pageSize = 10)
        {
            var query = _repo.GetStudentSchedulesQuery(studentId);

            if (dayOfWeek.HasValue)
                query = query.Where(cs => cs.DayOfWeek == dayOfWeek.Value);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(cs =>
                    cs.Class.ClassName.Contains(search) ||
                    cs.Class.Subject.Name.Contains(search) ||
                    cs.Class.Teacher.User.FullName.Contains(search) ||
                    cs.Room.Contains(search));
            }

            var list = await query
                .OrderBy(cs => cs.DayOfWeek)
                .ThenBy(cs => cs.StartTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return list.Select(cs => new StudentScheduleDto
            {
                ClassName = cs.Class.ClassName,
                SubjectName = cs.Class.Subject.Name,
                TeacherName = cs.Class.Teacher.User.FullName,
                DayOfWeek = cs.DayOfWeek,
                StartTime = cs.StartTime,
                EndTime = cs.EndTime,
                Room = cs.Room
            }).ToList();
        }
    }

}
