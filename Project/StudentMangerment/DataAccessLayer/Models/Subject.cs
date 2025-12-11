using DataAccessLayer.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Subject : BaseNameEntity
    {
        public int Credit { get; set; }
        public ICollection<StudentGrade> StudentGrades { get; set; } = new List<StudentGrade>();
        public ICollection<GradeComponent> GradeComponents { get; set; } = new List<GradeComponent>();

    }
}
