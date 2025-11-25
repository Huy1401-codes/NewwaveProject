using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Grade
    {
        [Key]
        public int GradeId { get; set; }

        [Required]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        [Required]
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        [Required]
        public int ClassId { get; set; }
        public Class Class { get; set; }

        [Range(0, 10)]
        public double MidtermScore { get; set; }

        [Range(0, 10)]
        public double FinalScore { get; set; }

        public double AverageScore { get; set; }
    }

}
