using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleTeacher;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.RoleTeacher
{
    public class ClassRepository : IClassRepository
    {
        private readonly SchoolContext _context;

        public ClassRepository(SchoolContext context)
        {
            _context = context;
        }


        /// <summary>
        /// danh sách lớp mà giao viên đảm nhận
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="search"></param>
        /// <param name="semesterId"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<Class>, int)> GetTeacherClassesAsync(
     int teacherId, int page, int pageSize, string search, int? semesterId)
        {
            var query = _context.Classes
                .Include(c => c.Subject)
                .Include(c => c.Semester)
                .Where(c => c.TeacherId == teacherId && c.IsStatus == true);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var loweredSearch = search.ToLower();

                query = query.Where(c =>
                    (c.Name != null && c.Name.ToLower().Contains(loweredSearch)) ||
                    (c.Subject != null && c.Subject.Name != null && c.Subject.Name.ToLower().Contains(loweredSearch))
                );
            }

            if (semesterId.HasValue)
                query = query.Where(c => c.SemesterId == semesterId);

            var total = await query.CountAsync();
            Console.WriteLine(semesterId);
            Console.WriteLine(query);
            Console.WriteLine(total);
            Console.WriteLine($"TeacherId: {teacherId}, Total classes: {total}");

            var data = await query
                .OrderByDescending(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, total);
        }


        public async Task<Class> GetByIdAsync(int classId)
        {
            return await _context.Classes
                .Include(c => c.Subject)
                .Include(c => c.Semester)
                .FirstOrDefaultAsync(c => c.Id == classId);
        }

        /// <summary>
        /// Teacher xem danh sách học sinh trong lớp
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public async Task<List<Class>> GetClassesByTeacherAsync(int teacherId)
        {
            return await _context.Classes
                .Include(c => c.Subject)
                .Where(c => c.TeacherId == teacherId)
                .ToListAsync();
        }

        public async Task<Class?> GetClassWithStudentsAsync(int classId)
        {
            return await _context.Classes
                .Include(c => c.ClassStudents)
                    .ThenInclude(cs => cs.Student)
                        .ThenInclude(s => s.User)
                .Include(c => c.Subject)
                .FirstOrDefaultAsync(c => c.Id == classId);
        }
    }

}
