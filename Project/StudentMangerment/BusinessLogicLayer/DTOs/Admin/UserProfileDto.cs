using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs.Admin
{
    public class UserProfileDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? ImageUrl { get; set; }
        public string TeacherCode { get; set; }
        public string StudentCode { get; set; }

        public string Role { get; set; }
    }


    public class UserProfileUpdateDto
    {
        public int UserId { get; set; }

        [StringLength(100)]
        public string? FullName { get; set; }  

        [EmailAddress, StringLength(100)]
        public string? Email { get; set; }

        [Phone, StringLength(20)]
        public string? Phone { get; set; }

        public string? TeacherCode { get; set; }
        public string? StudentCode { get; set; }

        public string? ImageUrl { get; set; }

        public IFormFile? ImageFile { get; set; }
    }


}
