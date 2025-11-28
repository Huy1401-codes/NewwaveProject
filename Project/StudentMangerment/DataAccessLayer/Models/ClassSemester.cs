using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ClassSemester
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClassId { get; set; }
        public Class Class { get; set; }

        [Required]
        public int SemesterId { get; set; }
        public Semester Semester { get; set; }

        public bool? IsStatus { get; set; } = true; // soft delete nếu cần

        // Navigation
        public ICollection<ClassSchedule> ClassSchedules { get; set; } = new List<ClassSchedule>();
        public ICollection<StudentGrade> StudentGrades { get; set; } = new List<StudentGrade>();
    }

}
