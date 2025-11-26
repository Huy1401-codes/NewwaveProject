using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Class
    {
        [Key]
        public int ClassId { get; set; }

        [Required, StringLength(50)]
        public string ClassName { get; set; }
        public bool? IsStatus { get; set; }

        [Required]
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        [Required]
        public int SemesterId { get; set; }
        public Semester Semester { get; set; }

        [Required]
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public ICollection<ClassStudent> ClassStudents { get; set; } = new List<ClassStudent>();

    }
}
