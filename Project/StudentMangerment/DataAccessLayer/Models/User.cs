using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required, StringLength(50)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [EmailAddress, StringLength(100)]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool? IsStatus { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
        public Student Student { get; set; }
        public Teacher Teacher { get; set; }
    }

}
