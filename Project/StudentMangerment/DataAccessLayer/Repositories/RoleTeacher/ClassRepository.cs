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
                .Where(c => c.TeacherId == teacherId && !c.IsStatus);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(c => c.ClassName.Contains(search)
                                      || c.Subject.Name.Contains(search));

            if (semesterId.HasValue)
                query = query.Where(c => c.SemesterId == semesterId);

            var total = await query.CountAsync();

            var data = await query
                .OrderByDescending(c => c.ClassId)
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
                .FirstOrDefaultAsync(c => c.ClassId == classId);
        }
    }

}
