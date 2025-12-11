using BusinessLogicLayer.DTOs.Results;
using BusinessLogicLayer.Messages.Admin;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.Services.RoleAdmin
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _account;
        private readonly ILogger<AccountService> _logger;


        public AccountService(IAccountRepository account, ILogger<AccountService> logger)
        {
            _account = account;
            _logger = logger;
        }

        public async Task<LoginResult> LoginAsync(string email, string password)
        {
            var result = new LoginResult();

            var user = await _account.GetByUsernameAsync(email);

            if (user == null)
            {
                result.ErrorMessage = LoginMessages.EmailFail;
                _logger.LogWarning(LoginMessages.EmailFail);
                return result;
            }

            if (user.IsStatus == false)
            {
                result.ErrorMessage = LoginMessages.InActive;
                _logger.LogWarning(LoginMessages.InActive);
                return result;
            }

            string storedPassword = user.PasswordHash;

            if (!string.IsNullOrEmpty(storedPassword) && storedPassword.StartsWith("$2"))
            {
                bool isMatch = BCrypt.Net.BCrypt.Verify(password, storedPassword);

                if (!isMatch)
                {
                    result.ErrorMessage = LoginMessages.PasswordFail;
                    return result;
                }

                result.User = user;
                return result;
            }

            if (storedPassword == password)
            {
                result.User = user;
                return result;
            }

            result.ErrorMessage = LoginMessages.PasswordFail;
            return result;
        }

    }
}
