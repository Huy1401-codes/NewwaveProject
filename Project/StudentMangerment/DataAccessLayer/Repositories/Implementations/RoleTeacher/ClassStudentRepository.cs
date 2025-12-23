using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleTeacher;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Implementations.RoleTeacher
{
    public class ClassStudentRepository : IClassStudentRepository
    {
        private readonly SchoolContext _context;

        public ClassStudentRepository(SchoolContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Danh sách học sinh từng lớp
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<Student>, int)> GetStudentsByClassAsync(
              int classId, int page, int pageSize, string search)
        {
            var query = _context.ClassStudents
                .Include(cs => cs.Student)
                    .ThenInclude(s => s.User)
                .Where(cs => cs.ClassId == classId
                    && cs.Student.User.IsStatus==true);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(cs =>
                    cs.Student.User.FullName.Contains(search.ToLower()) ||
                    cs.Student.StudentCode.Contains(search.ToLower()) ||
                    cs.Student.User.Email.Contains(search.ToLower())
                );
            }

            int total = await query.CountAsync();

            var students = await query
                .OrderBy(cs => cs.Student.User.FullName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(cs => cs.Student)
                .ToListAsync();

            return (students, total);
        }

        /// <summary>
        /// danh sách học sinh từng lớp
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public async Task<List<Student>> GetStudentsWithUserByClassAsync(int classId)
        {
            return await _context.ClassStudents
                .Include(cs => cs.Student).ThenInclude(s => s.User)
                .Where(cs => cs.ClassId == classId)
                .Select(cs => cs.Student)
                .ToListAsync();
        }


        public async Task<Student> GetStudentByIdAsync(int studentId)
        {
            return await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == studentId);
        }

    }

}
