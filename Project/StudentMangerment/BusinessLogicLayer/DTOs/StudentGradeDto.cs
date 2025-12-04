using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class StudentGradeDto
    {
        public string SubjectName { get; set; }
        public string ClassName { get; set; }
        public List<GradeDetail> Grades { get; set; }
        public double? AverageScore { get; set; } 

        public DateTime UpdateAt { get; set; }
    }
}
