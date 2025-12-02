using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs.ManagerStudent
{
    public class StudentPagedDto
    {
        public IEnumerable<Student> Students { get; set; }
        public int TotalCount { get; set; }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }
    }
}
