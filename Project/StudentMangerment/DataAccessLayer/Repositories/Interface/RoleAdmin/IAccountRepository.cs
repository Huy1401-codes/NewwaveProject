using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories.Interface.RoleAdmin
{
    public interface IAccountRepository
    {
        Task<User> GetByUsernameAsync(string email);
        Task<User> GetByIdAsync(int id);

        Task<RefreshToken> GetRefreshTokenAsync(string refreshToken);
        Task AddRefreshTokenAsync(RefreshToken refreshToken);
        Task RemoveOldRefreshTokensAsync(int userId);
    }
}
