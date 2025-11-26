using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs.Admin
{
    public class UserUpdateDto
    {
        public int UserId { get; set; }

        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool? IsStatus { get; set; }

        public int RoleId { get; set; }
    }

}
