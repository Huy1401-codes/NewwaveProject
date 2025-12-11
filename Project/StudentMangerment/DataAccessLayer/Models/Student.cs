using DataAccessLayer.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Student : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public string StudentCode { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }

        public string? Status { get; set; }

        public ICollection<ClassStudent> ClassStudents { get; set; } = new List<ClassStudent>();

        public ICollection<StudentGrade> StudentGrades { get; set; } = new List<StudentGrade>();

    }

}
