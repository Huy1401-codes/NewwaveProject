using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.Interface.RoleStudent;
using DataAccessLayer.Repositories.RoleStudent;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.RoleStudent
{
    public class StudentService : IStudentService
    {
        private readonly StudentRepository _repo;

        public StudentService(StudentRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Danh sách các lớp học theo thời gian của từng kì
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="search"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<StudentClassDto>> GetClassesAsync(
            int studentId,
            string? search = null,
            DateTime? from = null,
            DateTime? to = null,
            int page = 1,
            int pageSize = 10)
        {
            var query = _repo.GetStudentClassesQuery(studentId);

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

            // mapping sang DTO
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
            var grades = await _repo.GetStudentGradesQuery(studentId).ToListAsync();

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
