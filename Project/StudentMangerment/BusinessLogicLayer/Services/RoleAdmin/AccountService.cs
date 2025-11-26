using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.RoleAdmin
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _account;

        public AccountService(IAccountRepository account)
        {
            _account = account;
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            var user = await _account.GetByUsernameAsync(email);

            if (user == null) return null;
            if (user.PasswordHash != password) return null; 

            return user;
        }
    }
}
