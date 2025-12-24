using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Common.Messages
{
    public class AuthMessages
    {
        public const string UserNotFound = "User không tồn tại";
        public const string InvalidPassword = "Email hoặc mật khẩu không đúng";
        public const string LoginSuccess = "Đăng nhập thành công";

        public const string RefreshTokenInvalid = "Refresh token không hợp lệ";
        public const string RefreshTokenSuccess = "Refresh token thành công";

        public const string WrongOldPassword = "Mật khẩu cũ không đúng";
        public const string PasswordChangedSuccessfully = "Đổi mật khẩu thành công";
        public const string ForgotPasswordEmailNotFound = "Email không tồn tại";
        public const string NewPasswordGenerated = "Mật khẩu mới đã được tạo";

        public const string UnknownIp = "Unknown";
    }
}
