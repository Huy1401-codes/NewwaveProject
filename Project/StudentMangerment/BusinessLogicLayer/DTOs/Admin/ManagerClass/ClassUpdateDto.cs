using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs.Admin.ManagerClass
{
    public class ClassUpdateDto
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public bool IsStatus { get; set; } = true;

        public int SubjectId { get; set; }
        public int SemesterId { get; set; }
        public int TeacherId { get; set; }

        public List<int> StudentIds { get; set; } = new();
    }

}
