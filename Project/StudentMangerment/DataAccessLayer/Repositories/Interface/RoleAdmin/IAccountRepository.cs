using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interface.RoleAdmin
{
    public interface IAccountRepository
    {
        Task<User> GetByUsernameAsync(string email);
    }
}
