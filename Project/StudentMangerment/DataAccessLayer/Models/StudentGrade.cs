using DataAccessLayer.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class StudentGrade : BaseEntity
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public int ClassId { get; set; }
        public Class Class { get; set; }

        public int GradeComponentId { get; set; }
        public GradeComponent GradeComponent { get; set; }

        public int ClassSemesterId { get; set; }
        public ClassSemester ClassSemester { get; set; }

        public double? Score { get; set; }
    }
}
