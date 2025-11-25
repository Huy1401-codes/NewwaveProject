using BussinessLayer.DTOs;
using BussinessLayer.Services.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interrface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BussinessLayer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IUserRepository userRepo, IConfiguration config, IHttpContextAccessor httpContextAccessor )
        {
            _userRepo = userRepo;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }


        // Lấy IP client
        private string GetClientIp()
        {
            // Nếu chạy trong request HTTP, lấy IP thật
            var ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
            return ip ?? "Unknown"; 
        }

        /// <summary>
        /// đăng kí acc , có thể login bằng token dc sinh ra
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<AuthResponse> RegisterAsync(RegisterDto dto)
        {
            // 1. Kiểm tra email
            if (await _userRepo.GetByEmailAsync(dto.Email) != null)
                throw new Exception("Email đã tồn tại");

            // 2. Kiểm tra số điện thoại
            if (await _userRepo.GetByPhoneAsync(dto.PhoneNumber) != null)
                throw new Exception("Phone number already exists");

            // 3. Tạo user mới
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.PhoneNumber,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                PasswordSalt = BCrypt.Net.BCrypt.GenerateSalt(),
                IsActive = true,
                UserRoles = new List<UserRole> { new UserRole { RoleId = 3 } }, // Guest
                RefreshTokens = new List<RefreshToken>()
            };

            // 4. Tạo refresh token
            var refreshToken = new RefreshToken
            {
                TokenHash = Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                CreatedByIp = GetClientIp(), // lấy IP qua IHttpContextAccessor
            };
            user.RefreshTokens.Add(refreshToken);

            // 5. Lưu user
            await _userRepo.AddAsync(user);
            await _userRepo.SaveAsync();

            // 6. Tạo JWT token
            var token = GenerateJwtToken(user);

            return new AuthResponse
            {
                AccessToken = token,
                RefreshToken = refreshToken.TokenHash,
                ExpiredAt = refreshToken.ExpiresAt,
                User = new UserResponseDto
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = "Guest"
                }
            };
        }

        /// <summary>
        /// đăng nhập bằng jwt
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task<AuthResponse> LoginAsync(LoginDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "Login data cannot be null.");
            }

            // Kiểm tra nếu emailOrPhone là null hoặc rỗng
            if (string.IsNullOrEmpty(dto.EmailOrPhone))
            {
                throw new ArgumentException("Email or phone number is required.");
            }

            // Kiểm tra người dùng qua email hoặc số điện thoại
            var user = await _userRepo.GetByEmailAsync(dto.EmailOrPhone) ??
                       await _userRepo.GetByPhoneAsync(dto.EmailOrPhone);

            // Nếu không tìm thấy người dùng
            if (user == null)
            {
                // Return null hoặc ném exception tùy nhu cầu
                return null;
            }

            // Kiểm tra mật khẩu
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                // Nếu mật khẩu không khớp, return null hoặc ném exception
                return null;
            }

            // Khởi tạo RefreshTokens nếu chưa có
            if (user.RefreshTokens == null)
            {
                user.RefreshTokens = new List<RefreshToken>();  // Khởi tạo danh sách nếu null
            }

            // Tạo refresh token mới
            var refreshToken = new RefreshToken
            {
                TokenHash = Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                UserId = user.UserId,
                CreatedByIp = GetClientIp()
            };

            // Thêm refresh token vào danh sách
            user.RefreshTokens.Add(refreshToken);

            // Cập nhật user
            await _userRepo.UpdateAsync(user);
            await _userRepo.SaveAsync();

            // Tạo JWT token
            var token = GenerateJwtToken(user);

            return new AuthResponse
            {
                AccessToken = token,
                RefreshToken = refreshToken.TokenHash,
                ExpiredAt = refreshToken.ExpiresAt,
                User = new UserResponseDto
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = "Guest" // Hoặc lấy từ user.UserRoles tùy vào cách bạn quản lý roles
                }
            };
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordDto dto)
        {
            var user = await _userRepo.GetByIdAsync(dto.UserId);
            if (user == null) return false;

            if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.PasswordHash))
                return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _userRepo.UpdateAsync(user);

            return true;
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _userRepo.GetByEmailAsync(email);
            if (user == null) return false;

            string newPass = Guid.NewGuid().ToString().Substring(0, 8);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPass);

            await _userRepo.UpdateAsync(user);

            // Gửi email newPass cho người dùng
            // emailService.Send(email, "Your new password", newPass);

            return true;
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Kiểm tra nếu UserRoles là null hoặc rỗng
            var role = user.UserRoles != null && user.UserRoles.Any()
              ? user.UserRoles.FirstOrDefault()?.Role?.RoleName ?? "Guest"  // Dùng "Guest" nếu Role hoặc RoleName null
              : "Guest";  // Nếu không có role thì mặc định là Guest

            var claims = new[]
            {
           new Claim("id", user.UserId.ToString()),
           new Claim("role", role) // Thay vì user.UserRoles.ToString(), bạn dùng tên vai trò
           };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
        {
            // 1. Kiểm tra xem Refresh Token có hợp lệ và chưa hết hạn không
            var tokenInDb = await _userRepo.GetRefreshTokenAsync(refreshToken);
            if (tokenInDb == null || tokenInDb.ExpiresAt < DateTime.UtcNow || tokenInDb.RevokedAt != null)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");
            }

            // 2. Lấy user tương ứng với Refresh Token
            var user = await _userRepo.GetByIdAsync(tokenInDb.UserId);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }

            // 3. Tạo Access Token mới
            var newAccessToken = GenerateJwtToken(user);

            // 4. Tạo Refresh Token mới
            var newRefreshToken = new RefreshToken
            {
                TokenHash = Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.UtcNow.AddDays(7),  // Giả sử Refresh Token sống 7 ngày
                CreatedAt = DateTime.UtcNow,
                UserId = user.UserId,
                CreatedByIp = GetClientIp()
            };

            // 5. Cập nhật Refresh Token mới và xóa Refresh Token cũ nếu cần
            // Thêm Refresh Token mới vào cơ sở dữ liệu
            await _userRepo.AddRefreshTokenAsync(newRefreshToken);

            // 6. Xoá các Refresh Token cũ nếu cần
            await _userRepo.RemoveOldRefreshTokensAsync(user.UserId);

            // 7. Trả về Access Token mới và Refresh Token mới
            return new AuthResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.TokenHash,
                ExpiredAt = newRefreshToken.ExpiresAt,
                User = new UserResponseDto
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = "Guest" // Hoặc lấy từ user.UserRoles tùy vào cách bạn quản lý roles
                }
            };
        }

    }

}

