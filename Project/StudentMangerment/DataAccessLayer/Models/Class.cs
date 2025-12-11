using DataAccessLayer.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Class : BaseNameEntity
    {
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public int SemesterId { get; set; }
        public Semester Semester { get; set; }

        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public ICollection<ClassStudent> ClassStudents { get; set; } = new List<ClassStudent>();
        public ICollection<ClassSchedule> ClassSchedules { get; set; } = new List<ClassSchedule>(); 
        public ICollection<ClassSemester> ClassSemesters { get; set; } = new List<ClassSemester>();
        public ICollection<StudentGrade> StudentGrades { get; set; } = new List<StudentGrade>();

    }
}
