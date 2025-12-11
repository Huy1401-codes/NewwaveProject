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


        private string GetClientIp()
        {
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
            if (await _userRepo.GetByEmailAsync(dto.Email) != null)
                throw new Exception("Email đã tồn tại");

            if (await _userRepo.GetByPhoneAsync(dto.PhoneNumber) != null)
                throw new Exception("Phone number already exists");


            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.PhoneNumber,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                PasswordSalt = BCrypt.Net.BCrypt.GenerateSalt(),
                IsActive = true,
                UserRoles = new List<UserRole> { new UserRole { RoleId = 3 } }, 
                RefreshTokens = new List<RefreshToken>()
            };

            var refreshToken = new RefreshToken
            {
                TokenHash = Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                CreatedByIp = GetClientIp(),
            };
            user.RefreshTokens.Add(refreshToken);

            await _userRepo.AddAsync(user);
            await _userRepo.SaveAsync();

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

            if (string.IsNullOrEmpty(dto.EmailOrPhone))
            {
                throw new ArgumentException("Email or phone number is required.");
            }

            var user = await _userRepo.GetByEmailAsync(dto.EmailOrPhone) ??
                       await _userRepo.GetByPhoneAsync(dto.EmailOrPhone);

            if (user == null)
            {
                return null;
            }

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return null;
            }

            if (user.RefreshTokens == null)
            {
                user.RefreshTokens = new List<RefreshToken>(); 
            }

            var refreshToken = new RefreshToken
            {
                TokenHash = Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                UserId = user.UserId,
                CreatedByIp = GetClientIp()
            };

            user.RefreshTokens.Add(refreshToken);

            await _userRepo.UpdateAsync(user);
            await _userRepo.SaveAsync();

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

            
            return true;
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var role = user.UserRoles != null && user.UserRoles.Any()
              ? user.UserRoles.FirstOrDefault()?.Role?.RoleName ?? "Guest"  
              : "Guest";  

            var claims = new[]
            {
           new Claim("id", user.UserId.ToString()),
           new Claim("role", role)
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
            var tokenInDb = await _userRepo.GetRefreshTokenAsync(refreshToken);
            if (tokenInDb == null || tokenInDb.ExpiresAt < DateTime.UtcNow || tokenInDb.RevokedAt != null)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");
            }

            var user = await _userRepo.GetByIdAsync(tokenInDb.UserId);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }

            var newAccessToken = GenerateJwtToken(user);

            var newRefreshToken = new RefreshToken
            {
                TokenHash = Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.UtcNow.AddDays(7),  
                CreatedAt = DateTime.UtcNow,
                UserId = user.UserId,
                CreatedByIp = GetClientIp()
            };


            await _userRepo.AddRefreshTokenAsync(newRefreshToken);

            await _userRepo.RemoveOldRefreshTokensAsync(user.UserId);

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
                    Role = "Guest" 
                }
            };
        }

    }

}

