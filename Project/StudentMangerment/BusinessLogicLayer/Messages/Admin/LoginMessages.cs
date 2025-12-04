using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Messages.Admin
{
    public static class LoginMessages
    {
        public const string InActive = "Account had be block.";
        public const string EmailFail = "Email not found";
        public const string Success = "Login Successfully!";
        public const string PasswordFail = "Password Fail!";
    }
}
