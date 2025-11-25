using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.DTOs
{
    public class LoginDto
    {
        public string EmailOrPhone { get; set; }
        public string Password { get; set; }
    }

}
