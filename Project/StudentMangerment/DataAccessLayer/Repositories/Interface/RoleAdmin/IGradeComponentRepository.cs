using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interface.RoleAdmin
{
    public interface IGradeComponentRepository
    {
        Task<IEnumerable<GradeComponent>> GetBySubjectAsync(int subjectId);
        Task<GradeComponent> GetByIdAsync(int id);
        Task AddAsync(GradeComponent entity);
        Task UpdateAsync(GradeComponent entity);
        Task SoftDeleteAsync(int id);
    }

}
