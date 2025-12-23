using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using DataAccessLayer.Repositories.Implementations.Common;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Implementations.RoleAdmin
{
    public class ClassSemesterRepository
        : Repository<Class>, IClassSemesterRepository
    {
        public ClassSemesterRepository(SchoolContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Admin xem chi tiết lớp + học kỳ + GV + SV
        /// </summary>
        public async Task<Class?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(x => x.Semester)
                .Include(x => x.Subject)
                .Include(x => x.Teacher)
                    .ThenInclude(t => t.User)
                .Include(x => x.ClassStudents)
                    .ThenInclude(cs => cs.Student)
                        .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Admin xem toàn bộ danh sách lớp
        /// </summary>
        public async Task<List<Class>> GetAllAsync()
        {
            return await _dbSet
                .Include(x => x.Semester)
                .Include(x => x.Subject)
                .Include(x => x.Teacher)
                    .ThenInclude(t => t.User)
                .Include(x => x.ClassStudents)
                    .ThenInclude(cs => cs.Student)
                        .ThenInclude(s => s.User)
                .ToListAsync();
        }

    }
}
