using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Implementations.Common;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Implementations.RoleAdmin
{
    public class SemesterRepository : Repository<Semester>, ISemesterRepository
    {
        public SemesterRepository(SchoolContext context) : base(context) { }

        public IQueryable<Semester> GetQueryableOrdered()
            => _dbSet.AsNoTracking().OrderBy(s => s.Name);
    }
}
