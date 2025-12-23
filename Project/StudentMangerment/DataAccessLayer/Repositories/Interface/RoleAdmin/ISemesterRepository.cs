using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.Common;
using System.Linq.Expressions;

namespace DataAccessLayer.Repositories.Interface.RoleAdmin
{
    public interface ISemesterRepository : IRepository<Semester>
    {
        IQueryable<Semester> GetQueryableOrdered();
    }
}

