using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.RoleAdmin
{
    public class StudentRepository : IStudentRepository
    {
        private readonly SchoolContext _context;

        public StudentRepository(SchoolContext context)
        {
            _context = context;
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            return await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students
                .Include(s => s.User)
                .Where(s => !s.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(string role)
        {
            return await _context.Users.Include(a => a.UserRoles).ThenInclude(a => a.Role)
                .Where(u => u.UserRoles.Any(a => a.Role.Name == role) && u.IsStatus == true)
                .ToListAsync();
        }

        /// <summary>
        /// List student
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<Student> Data, int TotalCount)> GetPagedAsync(int page, int pageSize, string search)
        {
            var query = _context.Students
                .Include(s => s.User)
                .Where(s => !s.IsDeleted);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s =>
                s.User.FullName.Contains(search.ToLower()) ||
                s.User.Email.Contains(search.ToLower()) ||
                s.User.Phone.Contains(search.ToLower()) ||
                s.StudentCode.Contains(search.ToLower())
                );
            }

            int total = await query.CountAsync();

            var data = await query
                .OrderBy(s => s.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, total);
        }

        public async Task AddAsync(Student student)
        {
            await _context.Students.AddAsync(student);
        }

        public async Task UpdateAsync(Student student)
        {
            _context.Students.Update(student);
        }

        public async Task SoftDeleteAsync(int id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
            if (student != null)
            {
                student.IsDeleted = true;
                _context.Students.Update(student);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Student>> GetAllNameAsync()
        {
            return await _context.Students.Include(a => a.User).OrderBy(s => s.User.FullName).ToListAsync();
        }
    }
}
