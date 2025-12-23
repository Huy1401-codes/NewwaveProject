using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Implementations.Common;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Implementations.RoleAdmin
{
    public class StudentRepository
        : Repository<Student>, IStudentRepository
    {
        public StudentRepository(SchoolContext context) : base(context) { }


        public async Task<(IEnumerable<Student> Data, int TotalCount)>
            GetPagedAsync(int page, int pageSize, string search)
        {
            var query = _context.Students
                .Include(s => s.User)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s =>
                    s.User.FullName.Contains(search) ||
                    s.User.Email.Contains(search) ||
                    s.User.Phone.Contains(search) ||
                    s.StudentCode.Contains(search));
            }

            int total = await query.CountAsync();

            var data = await query
                .OrderBy(s => s.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, total);
        }

        public async Task<IEnumerable<Student>> GetAllNameAsync()
        {
            return await _context.Students
                .Include(s => s.User)
                .OrderBy(s => s.User.FullName)
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

        public async Task<Student?> GetByStudentCodeAsync(string studentCode)
        {
            return await _context.Students
                .FirstOrDefaultAsync(s => s.StudentCode == studentCode);
        }

    }
}
