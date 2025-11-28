using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs.Admin
{
    public class ClassCreateDto
    {
        public string ClassName { get; set; }

        public int SubjectId { get; set; }
        public int SemesterId { get; set; }
        public int TeacherId { get; set; }

        public List<int> StudentIds { get; set; }
    }

}
