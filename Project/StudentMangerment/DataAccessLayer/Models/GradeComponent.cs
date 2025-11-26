using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class GradeComponent
    {
        [Key]
        public int GradeComponentId { get; set; }

        [Required]
        [StringLength(100)]
        public string ComponentName { get; set; } // Giữa kỳ, Cuối kỳ, Bài tập...

        [Range(0, 1)]
        public double Weight { get; set; }  // hệ số (VD: 0.3)

        public bool IsDeleted { get; set; } = false;

        [Required]
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public ICollection<StudentGrade> StudentGrades { get; set; }
    }

}
