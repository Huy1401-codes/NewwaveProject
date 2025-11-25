using BussinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Services.Interface
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterDto dto);
        Task<AuthResponse> LoginAsync(LoginDto dto);
        Task<bool> ChangePasswordAsync(ChangePasswordDto dto);
        Task<bool> ForgotPasswordAsync(string email);

        Task<AuthResponse> RefreshTokenAsync(string refreshToken);
    }

}
