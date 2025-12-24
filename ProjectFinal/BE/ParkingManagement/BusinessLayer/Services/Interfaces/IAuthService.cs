using BusinessLayer.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);

        Task<LoginResponseDto> RefreshTokenAsync(string refreshToken);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ChangePasswordAsync(ChangePasswordDto dto);
    }

}
