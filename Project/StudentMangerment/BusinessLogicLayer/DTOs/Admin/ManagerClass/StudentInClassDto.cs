using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs.Admin.ManagerClass
{
    public class StudentInClassDto
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }

        public string StudentCode { get; set; } 
    }

}
