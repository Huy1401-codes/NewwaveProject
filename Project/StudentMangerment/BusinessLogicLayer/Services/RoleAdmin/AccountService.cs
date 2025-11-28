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
            if (user == null)
                return null;

            string storedPassword = user.PasswordHash;

            //1. Nếu mật khẩu đã mã hoá
            if (!string.IsNullOrEmpty(storedPassword) && storedPassword.StartsWith("$2"))
            {
                bool isMatch = BCrypt.Net.BCrypt.Verify(password, storedPassword);
                return isMatch ? user : null;
            }

            //2. Nếu mật khẩu chưa mã hoá  so sánh trực tiếp
            if (storedPassword == password)
            {
                return user;
            }

            return null;
        }


    }
}
