using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required, StringLength(20)]
        public string StudentCode { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [StringLength(10)]
        public string Gender { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string? Status { get; set; }
    }

}
