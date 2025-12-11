using DataAccessLayer.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class GradeComponent : BaseStatusEntity
    {
        public string ComponentName { get; set; }
        public double Weight { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public ICollection<StudentGrade> StudentGrades { get; set; }

    }


}
