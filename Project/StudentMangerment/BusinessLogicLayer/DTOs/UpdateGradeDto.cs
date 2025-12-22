using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class UpdateGradeDto
    {
        public int ClassId { get; set; }
        public int StudentId { get; set; }
        public Dictionary<int, double>? Scores { get; set; }
    }
}
