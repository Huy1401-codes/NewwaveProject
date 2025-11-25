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
        public int UserId { get; set; }

        [Required, MaxLength(200)]
        public string FullName { get; set; }

        [Required, MaxLength(200)]
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(50)]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
        public ICollection<MonthlyPass> MonthlyPasses { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; }

    }


}
