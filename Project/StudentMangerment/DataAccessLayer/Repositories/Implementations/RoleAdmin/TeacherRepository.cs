using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Implementations.Common;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Implementations.RoleAdmin
{
    public class TeacherRepository
     : Repository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(SchoolContext context)
            : base(context) { }

        public async Task<(IEnumerable<Teacher> Data, int TotalCount)>
            GetPagedAsync(int page, int pageSize, string search)
        {
            var query = _dbSet
                .Include(t => t.User)
                .Where(t => !t.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(t =>
                    t.User.FullName.Contains(search) ||
                    t.User.Email.Contains(search) ||
                    t.User.Phone.Contains(search) ||
                    t.TeacherCode.Contains(search));
            }

            int total = await query.CountAsync();

            var data = await query
                .OrderBy(t => t.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, total);
        }

        public async Task SoftDeleteAsync(int id)
        {
            var teacher = await GetByIdAsync(id);
            if (teacher != null)
            {
                teacher.IsDeleted = true;
                _dbSet.Update(teacher);
            }
        }

        public async Task<IEnumerable<Teacher>> GetAllNameAsync()
        {
            return await _dbSet
                .Include(t => t.User)
                .Where(t => !t.IsDeleted)
                .OrderBy(t => t.User.FullName)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(string role)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Where(u =>
                    u.UserRoles.Any(ur => ur.Role.Name == role) &&
                    u.IsStatus == true)
                .ToListAsync();
        }

        public async Task<int?> GetTeacherIdByUserIdAsync(int userId)
        {
            var teacher = await _dbSet
                .FirstOrDefaultAsync(t => t.UserId == userId && !t.IsDeleted);

            return teacher?.Id;
        }
    }

}
