using BusinessLogicLayer.Messages.Admin;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Context;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.Services.RoleAdmin
{

    public class ClassStudentRepository : IClassStudentRepository
    {
        private readonly SchoolContext _context;
        private readonly ILogger<ClassStudentRepository> _logger;

        public ClassStudentRepository(SchoolContext context, ILogger<ClassStudentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task AddStudentToClassAsync(ClassStudent entity)
        {
            try
            {
                _context.ClassStudents.Add(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ClassStudentMessages.AddError, entity.StudentId, entity.ClassId);
                throw;
            }
        }

        public async Task RemoveStudentFromClassAsync(int classId, int studentId)
        {
            try
            {
                var item = await _context.ClassStudents
                    .FirstOrDefaultAsync(x => x.ClassId == classId && x.StudentId == studentId);

                if (item != null)
                {
                    _context.ClassStudents.Remove(item);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ClassStudentMessages.RemoveError, studentId, classId);
                throw;
            }
        }

        public async Task<List<ClassStudent>> GetStudentsByClassIdAsync(int classId)
        {
            try
            {
                return await _context.ClassStudents
                    .Include(x => x.Student)
                    .Include(x => x.Class)
                    .Where(x => x.ClassId == classId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ClassStudentMessages.GetByClassError, classId);
                throw;
            }
        }


        public async Task<bool> ExistsAsync(int classId, int studentId)
        {
            try
            {
                return await _context.ClassStudents
                    .AnyAsync(x => x.ClassId == classId && x.StudentId == studentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ClassStudentMessages.ExistsError, studentId, classId);
                throw;
            }
        }
    }
}
