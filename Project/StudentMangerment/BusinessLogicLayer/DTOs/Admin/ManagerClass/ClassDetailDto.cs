using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs.Admin.ManagerClass
{
    public class ClassDetailDto
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


        public List<StudentInClassDto> Students { get; set; } = new List<StudentInClassDto>(); // chắc chắn khởi tạo
    }

}
