using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interface.RoleAdmin
{
    public interface ISemesterRepository
    {
        IQueryable<Semester> GetAllQueryable();
        Task<Semester> GetByIdAsync(int id);
        Task AddAsync(Semester semester);
        Task UpdateAsync(Semester semester);
        Task SaveAsync();

        Task<IEnumerable<Semester>> GetAllAsync();

        Task<bool> AnyAsync(Expression<Func<Semester, bool>> predicate);
    }
}
