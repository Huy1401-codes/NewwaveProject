using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs.ManagerTeacher
{
    public class EnterGradeViewModel
    {
        public int ClassId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public List<GradeComponentDto> Components { get; set; }
    }

    public class GradeComponentDto
    {
        public int GradeComponentId { get; set; }
        public string ComponentName { get; set; }
        public double Weight { get; set; }

        public double? Score { get; set; } // thêm Score

    }

}
