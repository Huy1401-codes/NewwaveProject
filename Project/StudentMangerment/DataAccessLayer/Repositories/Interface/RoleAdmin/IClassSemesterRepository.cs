using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interface.RoleAdmin
{
    public interface IClassSemesterRepository
    {
        Task<Class> GetByIdAsync(int id);
        Task<List<Class>> GetAllAsync();
        Task AddAsync(Class entity);
        Task UpdateAsync(Class entity);
        Task DeleteAsync(int id);
    }
}
