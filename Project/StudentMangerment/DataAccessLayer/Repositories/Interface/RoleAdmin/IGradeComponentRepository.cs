using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interface.RoleAdmin
{
    public interface IGradeComponentRepository : IRepository<GradeComponent>
    {
        Task<IEnumerable<GradeComponent>> GetBySubjectAsync(int subjectId);
        Task SoftDeleteAsync(int id);
    }

}
