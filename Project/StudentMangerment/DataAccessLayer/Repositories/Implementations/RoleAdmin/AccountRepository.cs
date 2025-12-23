using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Implementations.Common;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Implementations.RoleAdmin
{
    public class AccountRepository
        : Repository<User>, IAccountRepository
    {
        public AccountRepository(SchoolContext context)
            : base(context)
        {
        }

        public async Task<User?> GetByUsernameAsync(string email)
        {
            return await _dbSet
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken)
        {
            return await _context.Set<RefreshToken>()
                .FirstOrDefaultAsync(t =>
                    t.TokenHash == refreshToken &&
                    t.IsActive);
        }

        public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _context.Set<RefreshToken>().AddAsync(refreshToken);
        }

        public async Task RemoveOldRefreshTokensAsync(int userId)
        {
            var expiredTokens = await _context.Set<RefreshToken>()
                .Where(t =>
                    t.UserId == userId &&
                    t.ExpiresAt < DateTime.UtcNow &&
                    t.RevokedAt == null)
                .ToListAsync();

            _context.Set<RefreshToken>().RemoveRange(expiredTokens);
        }
    }
}
