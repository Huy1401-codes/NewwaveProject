using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interrface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ParkingDbContext _context;

        public UserRepository(ParkingDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lấy tài khoản theo email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<User> GetByEmailAsync(string email) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        /// <summary>
        /// Lấy acc theo phone
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public async Task<User> GetByPhoneAsync(string phone) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Phone == phone);

        /// <summary>
        /// Lấy acc theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public async Task<User> GetByIdAsync(int id) =>
            await _context.Users.FindAsync(id);

        /// <summary>
        /// Tạo acc mới
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task AddAsync(User user) =>
            await _context.Users.AddAsync(user);

        /// <summary>
        /// Cập nhật thông tin acc
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Lưu thông tin vào db
        /// </summary>
        /// <returns></returns>
        public Task SaveAsync() => _context.SaveChangesAsync();



        /// <summary>
        /// tìm kiếm Refresh Token tron db
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public async Task<RefreshToken> GetRefreshTokenAsync(string refreshToken)
        {
            // Lấy RefreshToken từ database, sử dụng FirstOrDefaultAsync() để thực hiện truy vấn bất đồng bộ
            var refreshTokenEntity = await _context.RefreshTokens
                                                    .Where(t => t.TokenHash == refreshToken)
                                                    .FirstOrDefaultAsync();  // Không cần await nữa, vì là một đối tượng RefreshToken

            // Kiểm tra trạng thái của RefreshToken
            if (refreshTokenEntity != null && refreshTokenEntity.IsActive)
            {
                return refreshTokenEntity;  // Trả về RefreshToken nếu nó hợp lệ
            }

            return null;  // Nếu token không hợp lệ hoặc không tồn tại
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
