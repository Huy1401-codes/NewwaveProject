using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Context;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services.RoleAdmin
{

    public class ClassStudentRepository : IClassStudentRepository
    {
        private readonly SchoolContext _context;

        public ClassStudentRepository(SchoolContext context)
        {
            _context = context;
        }

        public async Task AddStudentToClassAsync(ClassStudent entity)
        {
            _context.ClassStudents.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveStudentFromClassAsync(int classId, int studentId)
        {
            var item = await _context.ClassStudents
                .FirstOrDefaultAsync(x => x.ClassId == classId && x.StudentId == studentId);

            if (item != null)
            {
                _context.ClassStudents.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ClassStudent>> GetStudentsByClassIdAsync(int classId)
        {
            return await _context.ClassStudents
                .Include(x => x.Student)
                .Include(x => x.Class)
                .Where(x => x.ClassId == classId)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int classId, int studentId)
        {
            return await _context.ClassStudents
                .AnyAsync(x => x.ClassId == classId && x.StudentId == studentId);
        }
    }
}
