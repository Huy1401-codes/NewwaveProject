using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class StudentGrade
    {
        [Key]
        public int StudentGradeId { get; set; }

        [Required]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        [Required]
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        [Required]
        public int ClassId { get; set; }
        public Class Class { get; set; }

        [Required]
        public int GradeComponentId { get; set; }
        public GradeComponent GradeComponent { get; set; }

        [Required]
        public int ClassSemesterId { get; set; }   // mới
        public ClassSemester ClassSemester { get; set; }  // navigation property

        [Range(0, 10)]
        public double? Score { get; set; }  // có thể null nếu chưa nhập

        public DateTime UpdatedAt { get; set; }
    }

}
