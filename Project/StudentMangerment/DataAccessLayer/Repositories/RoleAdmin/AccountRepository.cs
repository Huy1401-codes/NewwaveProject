using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.RoleAdmin
{
    public class AccountRepository : IAccountRepository
    {
        private readonly SchoolContext _context;
        public AccountRepository(SchoolContext context)
        {
            _context = context;
        }

        public async Task<User> GetByUsernameAsync(string email)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }


        public async Task<User> GetByIdAsync(int id) =>
           await _context.Users.FindAsync(id);


        /// <summary>
        /// tìm kiếm Refresh Token tron db
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public async Task<RefreshToken> GetRefreshTokenAsync(string refreshToken)
        {
            var refreshTokenEntity = await _context.RefreshTokens
                                                    .Where(t => t.TokenHash == refreshToken)
                                                    .FirstOrDefaultAsync();

            if (refreshTokenEntity != null && refreshTokenEntity.IsActive)
            {
                return refreshTokenEntity;
            }

            return null;
        }





        /// <summary>
        /// lưu Refresh Token mới
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// xóa các Refresh Token cũ hoặc đã hết hạn
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task RemoveOldRefreshTokensAsync(int userId)
        {
            var expiredTokens = await _context.RefreshTokens
                .Where(t => t.UserId == userId && t.ExpiresAt < DateTime.UtcNow && t.RevokedAt == null)
                .ToListAsync();

            _context.RefreshTokens.RemoveRange(expiredTokens);
            await _context.SaveChangesAsync();
        }

    }
}
