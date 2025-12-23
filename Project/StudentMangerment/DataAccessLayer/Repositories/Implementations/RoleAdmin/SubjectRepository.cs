using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Implementations.Common;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Implementations.RoleAdmin
{

    public class SubjectRepository
        : Repository<Subject>, ISubjectRepository
    {
        public SubjectRepository(SchoolContext context)
            : base(context) { }

        public IQueryable<Subject> GetActiveSubjects()
            => _dbSet.Where(s => s.IsStatus == true);

        public async Task SoftDeleteAsync(int id)
        {
            var subject = await GetByIdAsync(id);
            if (subject != null)
            {
                subject.IsStatus = false;
                _dbSet.Update(subject);
            }
        }

      
    }
}