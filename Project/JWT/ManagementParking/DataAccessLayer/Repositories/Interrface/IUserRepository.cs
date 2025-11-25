using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interrface
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByPhoneAsync(string phone);
        Task<User> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task SaveAsync();
            
        //lấy token
        Task<RefreshToken> GetRefreshTokenAsync(string refreshToken);
        Task AddRefreshTokenAsync(RefreshToken refreshToken);
        Task RemoveOldRefreshTokensAsync(int userId);
    }

}
