using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interface.RoleAdmin
{
    public interface ISubjectRepository
    {
        IQueryable<Subject> GetAllQueryable();
        Task<Subject> GetByIdAsync(int id);
        Task AddAsync(Subject sub);
        Task UpdateAsync(Subject sub);
        Task SoftDeleteAsync(int id);
        Task SaveAsync();

        Task<IEnumerable<Subject>> GetAllAsync();
        Task<bool> AnyAsync(Expression<Func<Subject, bool>> predicate);

    }


}
