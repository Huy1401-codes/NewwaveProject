using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.DTOs
{
    /// <summary>
    /// Gửi thông tin xác thực từ server về client sau khi đăng nhập hoặc refresh token
    /// </summary>
    public class AuthResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiredAt { get; set; }

        public UserResponseDto User { get; set; }

    }

}
