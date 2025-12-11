using DataAccessLayer.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ClassSemester : BaseStatusEntity
    {
        public int ClassId { get; set; }
        public Class Class { get; set; }

        public int SemesterId { get; set; }
        public Semester Semester { get; set; }

        public ICollection<ClassSchedule> ClassSchedules { get; set; } = new List<ClassSchedule>();
        public ICollection<StudentGrade> StudentGrades { get; set; } = new List<StudentGrade>();
    }
}
