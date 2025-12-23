using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Admin;
using BusinessLogicLayer.DTOs.Admin.Jwt;
using BusinessLogicLayer.DTOs.Results;

namespace BusinessLogicLayer.Services.Interface.RoleAdmin
{
    public interface IAccountService
    {
        Task<LoginResult> LoginAsync(string email, string password);

        Task<JwtLoginResponseDto> LoginJwtAsync(string email, string password);
        Task<AuthResponse> RefreshTokenAsync(string refreshToken);

        Task<UserProfileDto?> GetMyProfileAsync(int userId);

        Task<(bool Success, string ErrorMessage)> UpdateProfileAsync(UserProfileUpdateDto dto);

    }
}
