using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs.Admin
{
    public class ClassDto
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public bool? IsStatus { get; set; }

        public int SubjectId { get; set; }
        public string SubjectName { get; set; }

        public int SemesterId { get; set; }
        public string SemesterName { get; set; }

        public int TeacherId { get; set; }
        public string TeacherName { get; set; }

        public List<StudentDto> Students { get; set; } = new();
    }

}
