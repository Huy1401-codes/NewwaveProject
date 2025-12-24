using AutoMapper;
using BusinessLayer.Common.Messages;
using BusinessLayer.DTOs.Auth;
using BusinessLayer.Helpers.Interface;
using BusinessLayer.Services.Interfaces;
using DataAccessLayer.UnitOfWork;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BussinessLayer.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenHelper _jwtTokenHelper;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUnitOfWork unitOfWork,
            IJwtTokenHelper jwtTokenHelper,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<AuthService> logger)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenHelper = jwtTokenHelper;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        private string GetClientIp()
        {
            return _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString()
                   ?? AuthMessages.UnknownIp;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            _logger.LogInformation("Login attempt for {Email}", request.Email);

            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);

            if (user == null)
            {
                _logger.LogWarning("Login failed: user not found for {Email}", request.Email);
                return null;
            }

            bool passwordValid = false;
            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                passwordValid = user.PasswordHash.StartsWith("$2")
                    ? BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)
                    : request.Password == user.PasswordHash;
            }

            if (!passwordValid)
            {
                _logger.LogWarning("Login failed: invalid password for {Email}", request.Email);
                return null;
            }

            var refreshToken = new RefreshToken
            {
                TokenHash = Guid.NewGuid().ToString(),
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                CreatedByIp = GetClientIp()
            };

            await _unitOfWork.RefreshTokens.AddAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            var accessToken = _jwtTokenHelper.GenerateAccessToken(user);

            _logger.LogInformation("Login successful for {Email}", request.Email);

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.TokenHash,
                User = _mapper.Map<UserLoginDto>(user)
            };
        }

        public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken)
        {
            _logger.LogInformation("Refresh token attempt");

            var tokenEntity = (await _unitOfWork.RefreshTokens.FindAsync(
                x => x.TokenHash == refreshToken && x.RevokedAt == null))
                .FirstOrDefault();

            if (tokenEntity == null || tokenEntity.ExpiresAt < DateTime.UtcNow)
            {
                _logger.LogWarning("Refresh token invalid or expired");
                throw new UnauthorizedAccessException(AuthMessages.RefreshTokenInvalid);
            }

            var user = await _unitOfWork.Users.GetByIdAsync(tokenEntity.UserId);
            if (user == null)
            {
                _logger.LogWarning("Refresh token failed: user not found");
                throw new UnauthorizedAccessException(AuthMessages.UserNotFound);
            }

            tokenEntity.RevokedAt = DateTime.UtcNow;
            tokenEntity.RevokedByIp = GetClientIp();
            _unitOfWork.RefreshTokens.Update(tokenEntity);

            var newRefreshToken = new RefreshToken
            {
                TokenHash = Guid.NewGuid().ToString(),
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                CreatedByIp = GetClientIp()
            };
            await _unitOfWork.RefreshTokens.AddAsync(newRefreshToken);

            var newAccessToken = _jwtTokenHelper.GenerateAccessToken(user);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Refresh token successful for user {UserId}", user.Id);

            return new LoginResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.TokenHash,
                User = _mapper.Map<UserLoginDto>(user)
            };
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordDto dto)
        {
            _logger.LogInformation("Change password attempt for user {UserId}", dto.UserId);

            var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
            if (user == null)
            {
                _logger.LogWarning("Change password failed: user not found {UserId}", dto.UserId);
                return false;
            }

            if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.PasswordHash))
            {
                _logger.LogWarning("Change password failed: wrong old password {UserId}", dto.UserId);
                return false;
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            _unitOfWork.Users.Update(user);

            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Password changed successfully for user {UserId}", dto.UserId);

            return true;
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            _logger.LogInformation("Forgot password attempt for {Email}", email);

            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("Forgot password failed: email not found {Email}", email);
                return false;
            }

            string newPass = Guid.NewGuid().ToString().Substring(0, 8);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPass);
            _unitOfWork.Users.Update(user);

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("New password generated for {Email}", email);
            return true;
        }
    }
}
