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
                .FirstOrDefaultAsync(s => s.StudentId == id && !s.IsDeleted);
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students
                .Include(s => s.User)
                .Where(s => !s.IsDeleted)
                .ToListAsync();
        }

        /// <summary>
        /// Danh sách học sinh
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
                    s.User.FullName.Contains(search) ||
                    s.User.Username.Contains(search));
            }

            int total = await query.CountAsync();

            var data = await query
                .OrderBy(s => s.StudentId)
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
            var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == id);
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
    }
}
