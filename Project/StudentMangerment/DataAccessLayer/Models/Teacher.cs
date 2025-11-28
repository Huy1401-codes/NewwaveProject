using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required, StringLength(20)]
        public string TeacherCode { get; set; }

        [StringLength(50)]
        public string Degree { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string? Status { get; set; }

        [NotMapped]
        public string FullName => User?.FullName ?? "[No Name]";
    }
}
