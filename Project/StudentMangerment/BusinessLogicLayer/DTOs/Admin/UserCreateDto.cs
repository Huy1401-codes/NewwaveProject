using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs.Admin
{
    public class UserCreateDto
    {
        public string Username { get; set; }
        public string FullName { get; set; }

        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool? IsStatus { get; set; }

        public int RoleId { get; set; }  // Chỉ 1 vai trò
    }

}
