using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.Common;

namespace DataAccessLayer.Repositories.Interface.RoleAdmin
{
    public interface IAccountRepository : IRepository<User>
    {
        Task<User?> GetByUsernameAsync(string email);

        Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken);
        Task AddRefreshTokenAsync(RefreshToken refreshToken);
        Task RemoveOldRefreshTokensAsync(int userId);
    }

}
