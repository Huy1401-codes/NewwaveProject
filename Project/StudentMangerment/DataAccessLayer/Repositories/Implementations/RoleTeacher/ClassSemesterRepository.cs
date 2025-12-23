using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleTeacher;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Implementations.RoleTeacher
{
    public class ClassSemesterRepository : IClassSemesterRepository
    {
        private readonly SchoolContext _context;

        public ClassSemesterRepository(SchoolContext context)
        {
            _context = context;
        }

        public async Task<ClassSemester> GetClassSemesterAsync(int classId, int? semesterId = null)
        {
            var query = _context.ClassSemesters.AsQueryable();

            query = query.Where(cs => cs.ClassId == classId);

            if (semesterId.HasValue)
            {
                query = query.Where(cs => cs.SemesterId == semesterId.Value);
            }

            return await query.FirstOrDefaultAsync();
        }
    }

}
