using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Implementations.Common;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Implementations.RoleAdmin
{
    public class ClassStudentRepository
        : Repository<ClassStudent>, IClassStudentRepository
    {
        public ClassStudentRepository(SchoolContext context) : base(context)
        {
        }

        public async Task AddStudentToClassAsync(int classId, int studentId)
        {
            var exists = await _dbSet
                .AnyAsync(cs => cs.ClassId == classId && cs.StudentId == studentId);

            if (!exists)
            {
                await _dbSet.AddAsync(new ClassStudent
                {
                    ClassId = classId,
                    StudentId = studentId
                });
            }
        }

        public async Task RemoveStudentFromClassAsync(int classId, int studentId)
        {
            var cs = await _dbSet
                .FirstOrDefaultAsync(x => x.ClassId == classId && x.StudentId == studentId);

            if (cs != null)
                _dbSet.Remove(cs);
        }

        public async Task<List<ClassStudent>> GetStudentsByClassIdAsync(int classId)
        {
            return await _context.ClassStudents
                .Where(cs => cs.ClassId == classId)
                .Include(cs => cs.Student)
                    .ThenInclude(s => s.User)
                .ToListAsync();
        }
    }
}
