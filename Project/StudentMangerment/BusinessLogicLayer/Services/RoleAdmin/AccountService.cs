using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Admin.Jwt;
using BusinessLogicLayer.DTOs.Results;
using BusinessLogicLayer.Messages.Admin;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLogicLayer.Services.RoleAdmin
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _account;
        private readonly ILogger<AccountService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;


        public AccountService(IAccountRepository account, ILogger<AccountService> logger, 
                    IHttpContextAccessor httpContextAccessor, IConfiguration configuration )
        {
            _account = account;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _config = configuration;
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


        private string GetClientIp()
        {
            var ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
            return ip ?? "Unknown";
        }
        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var role = user.UserRoles != null && user.UserRoles.Any()
              ? user.UserRoles.FirstOrDefault()?.Role?.Name ?? "Guest"
              : "Guest";

            var claims = new[]
            {
           new Claim("id", user.Id.ToString()),
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
            var tokenInDb = await _account.GetRefreshTokenAsync(refreshToken);
            if (tokenInDb == null || tokenInDb.ExpiresAt < DateTime.UtcNow || tokenInDb.RevokedAt != null)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");
            }

            var user = await _account.GetByIdAsync(tokenInDb.UserId);
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
                UserId = user.Id,
                CreatedByIp = GetClientIp()
            };


            await _account.AddRefreshTokenAsync(newRefreshToken);

            await _account.RemoveOldRefreshTokensAsync(user.Id);

            return new AuthResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.TokenHash,
                ExpiredAt = newRefreshToken.ExpiresAt,
                User = new UserResponseDto
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = "Guest"
                }
            };
        }

        public async Task<JwtLoginResponseDto> LoginJwtAsync(string email, string password)
        {
            var user = await _account.GetByUsernameAsync(email);
            if (user == null || user.IsStatus == false)
                throw new UnauthorizedAccessException("Email hoặc mật khẩu không đúng");

            bool isPasswordValid = false;

            if (!string.IsNullOrEmpty(user.PasswordHash) && user.PasswordHash.StartsWith("$2"))
                isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            else
                isPasswordValid = user.PasswordHash == password;

            if (!isPasswordValid)
                throw new UnauthorizedAccessException("Email hoặc mật khẩu không đúng");

            // tạo JWT access token
            var accessToken = GenerateJwtToken(user);

            // tạo refresh token
            var refreshToken = new RefreshToken
            {
                TokenHash = Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                UserId = user.Id,
                CreatedByIp = GetClientIp()
            };
            await _account.AddRefreshTokenAsync(refreshToken);
            await _account.RemoveOldRefreshTokensAsync(user.Id);

            return new JwtLoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.TokenHash,
                ExpiredAt = refreshToken.ExpiresAt,
                User = new JwtUserDto
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = user.UserRoles.FirstOrDefault()?.Role?.Name ?? "Guest"
                }
            };
        }




    }
}
