using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Implementations.Common;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccessLayer.Repositories.Implementations.RoleAdmin
{
    public class GradeComponentRepository : Repository<GradeComponent>, IGradeComponentRepository
    {
        public GradeComponentRepository(SchoolContext context) : base(context) { }

        public async Task<IEnumerable<GradeComponent>> GetBySubjectAsync(int subjectId)
        {
            return await _dbSet.Where(x => x.SubjectId == subjectId && !x.IsDeleted)
                               .ToListAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                entity.UpdatedAt = DateTime.Now;
            }
        }
    }
}
