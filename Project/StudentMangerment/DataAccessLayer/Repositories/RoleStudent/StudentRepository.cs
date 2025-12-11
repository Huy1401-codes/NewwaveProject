using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleStudent;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.RoleStudent
{
    public class StudentRepository : IStudentRepository
    {
        private readonly SchoolContext _context;

        public StudentRepository(SchoolContext context)
        {
            _context = context;
        }

        /// <summary>
        /// danh sách ClassStudent
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public IQueryable<ClassStudent> GetStudentClassesQuery(int studentId)
        {
            return _context.ClassStudents
                .Where(cs => cs.StudentId == studentId)
                .Include(cs => cs.Class)
                    .ThenInclude(c => c.Subject)
                .Include(cs => cs.Class)
                    .ThenInclude(c => c.Semester)
                .Include(cs => cs.Class)
                    .ThenInclude(c => c.Teacher)
                        .ThenInclude(t => t.User);
        }


        /// <summary>
        /// Lấy tất cả StudentGrade của student
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public IQueryable<StudentGrade> GetStudentGradesQuery(int studentId)
        {
            return _context.StudentGrades
                .Include(g => g.Class)
                .Include(g => g.Subject)
                .Include(g => g.GradeComponent).Include(a=>a.ClassSemester).ThenInclude(a=>a.Semester)
                .Where(g => g.StudentId == studentId);
        }

        /// <summary>
        /// Lịch học của học sinh
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public IQueryable<ClassSchedule> GetStudentSchedulesQuery(int studentId)
        {
            return _context.ClassSchedules
                .Include(cs => cs.Class)
                    .ThenInclude(c => c.Subject)
                .Include(cs => cs.Class)
                    .ThenInclude(c => c.Teacher)
                        .ThenInclude(t => t.User)
                .Where(cs => cs.Class.ClassStudents.Any(cs2 => cs2.StudentId == studentId));
        }

        public async Task<int?> GetStudentIdByUserIdAsync(int userId)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.UserId == userId);

            return student?.Id;
        }
    }
}
