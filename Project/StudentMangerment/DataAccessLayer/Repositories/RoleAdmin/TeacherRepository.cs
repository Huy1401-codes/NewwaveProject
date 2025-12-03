using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.RoleAdmin
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly SchoolContext _context;

        public TeacherRepository(SchoolContext context)
        {
            _context = context;
        }

        public async Task<Teacher> GetByIdAsync(int id)
        {
            return await _context.Teachers
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.TeacherId == id && !t.IsDeleted);
        }

        public async Task<IEnumerable<Teacher>> GetAllAsync()
        {
            return await _context.Teachers
                .Include(t => t.User)
                .Where(t => !t.IsDeleted)
                .ToListAsync();
        }

        /// <summary>
        /// Danh sách giao viên
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<Teacher> Data, int TotalCount)> GetPagedAsync(int page, int pageSize, string search)
        {
            var query = _context.Teachers
                .Include(t => t.User)
                .Where(t => !t.IsDeleted);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t =>
                     t.User.FullName.Contains(search.ToLower()) ||
                     t.User.Email.Contains(search.ToLower()) ||
                     t.User.Phone.Contains(search.ToLower()) ||
                     t.TeacherCode.Contains(search.ToLower())
                    );
            }

            int total = await query.CountAsync();

            var data = await query
                .OrderBy(t => t.TeacherId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, total);
        }

        public async Task AddAsync(Teacher teacher)
        {
            await _context.Teachers.AddAsync(teacher);
        }

        public async Task UpdateAsync(Teacher teacher)
        {
            _context.Teachers.Update(teacher);
        }

        public async Task SoftDeleteAsync(int id)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.TeacherId == id);
            if (teacher != null)
            {
                teacher.IsDeleted = true;
                _context.Teachers.Update(teacher);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Teacher>> GetAllNameAsync()
        {
            return await _context.Teachers.Include(a => a.User).OrderBy(t => t.User.FullName).ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(string role)
        {
            return await _context.Users.Include(a => a.UserRoles).ThenInclude(a => a.Role)
                .Where(u => u.UserRoles.Any(a => a.Role.RoleName == role) && u.IsStatus == true)
                .ToListAsync();
        }
    }
}
