using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interface.RoleAdmin
{
    public interface IClassSemesterRepository : IRepository<Class>
    {
        Task<Class?> GetByIdAsync(int id);
        Task<List<Class>> GetAllAsync();
    }
}
