using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.RoleAdmin
{
    public class ClassStudentRepository : IClassStudentRepository
    {
        private readonly SchoolContext _context;

        public ClassStudentRepository(SchoolContext context)
        {
            _context = context;
        }

        public async Task AddStudentToClassAsync(int classId, int studentId)
        {
            var exists = await _context.ClassStudents
                .AnyAsync(cs => cs.ClassId == classId && cs.StudentId == studentId);
            if (!exists)
            {
                await _context.ClassStudents.AddAsync(new ClassStudent
                {
                    ClassId = classId,
                    StudentId = studentId
                });
            }
        }

        public async Task RemoveStudentFromClassAsync(int classId, int studentId)
        {
            var cs = await _context.ClassStudents
                .FirstOrDefaultAsync(x => x.ClassId == classId && x.StudentId == studentId);

            if (cs != null)
            {
                _context.ClassStudents.Remove(cs);
            }
        }

        public async Task<List<ClassStudent>> GetStudentsByClassIdAsync(int classId)
        {
            return await _context.ClassStudents
                .Where(cs => cs.ClassId == classId)
                .Include(cs => cs.Student)
                .ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
