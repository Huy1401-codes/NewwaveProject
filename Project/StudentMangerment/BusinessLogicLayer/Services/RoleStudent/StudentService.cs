using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Messages.Student;
using BusinessLogicLayer.Services.Interface.RoleStudent;
using DataAccessLayer.Repositories.Interface.RoleStudent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.Services.RoleStudent
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repo;
        private readonly ILogger<StudentService> _logger;

        public StudentService(IStudentRepository repo, ILogger<StudentService> logger)
        {
            _repo = repo;
            _logger = logger;
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
            {
                _logger.LogWarning(StudentMessages.StudentNotFound, userId);
                return new List<StudentClassDto>();
            }

            var query = _repo.GetStudentClassesQuery(studentId.Value);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(cs =>
                    cs.Class.ClassName.Contains(search.ToLower()) ||
                    cs.Class.Subject.Name.Contains(search.ToLower()) ||
                    cs.Class.Teacher.User.FullName.Contains(search.ToLower()));
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

            _logger.LogInformation(StudentMessages.ListStudent, userId, list.Count);

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

        public async Task<List<StudentGradeDto>> GetGradesAsync(int userId)
        {
            var studentId = await _repo.GetStudentIdByUserIdAsync(userId);
            if (studentId == null)
            {
                _logger.LogWarning(StudentMessages.StudentNotFound, userId);
                return new List<StudentGradeDto>();
            }

            var grades = await _repo.GetStudentGradesQuery(studentId.Value).ToListAsync();

            if (!grades.Any())
            {
                _logger.LogWarning(StudentMessages.GradeNotFound, studentId);
                return new List<StudentGradeDto>();
            }

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
                    AverageScore = g.Sum(x => (x.Score ?? 0) * x.GradeComponent.Weight)
                                   / g.Sum(x => x.GradeComponent.Weight)
                }).ToList();

            _logger.LogWarning(StudentMessages.GradeNotFound, studentId);

            return result;
        }

        public async Task<List<StudentScheduleDto>> GetStudentSchedulesAsync(
            int studentId,
            int? dayOfWeek = null,
            string? search = null,
            int page = 1,
            int pageSize = 10)
        {
            var query = _repo.GetStudentSchedulesQuery(studentId);

            if (!query.Any())
            {
                _logger.LogWarning(StudentMessages.ScheduleNotFound, studentId);
            }

            if (dayOfWeek.HasValue)
                query = query.Where(cs => cs.DayOfWeek == dayOfWeek.Value);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(cs =>
                    cs.Class.ClassName.Contains(search.ToLower()) ||
                    cs.Class.Subject.Name.Contains(search.ToLower()) ||
                    cs.Class.Teacher.User.FullName.Contains(search.ToLower()) ||
                    cs.Room.Contains(search));
            }

            var list = await query
                .OrderBy(cs => cs.DayOfWeek)
                .ThenBy(cs => cs.StartTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            _logger.LogInformation(StudentMessages.ScheduleListInfo, studentId, list.Count);

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
