using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interface.RoleAdmin
{
    public interface IClassRepository
    {
        IQueryable<Class> GetAllQueryable();
        Task<Class> GetByIdAsync(int id);
        Task AddAsync(Class cls);
        Task UpdateAsync(Class cls);
        Task SoftDeleteAsync(int id);
        Task SaveAsync();
    }

}
