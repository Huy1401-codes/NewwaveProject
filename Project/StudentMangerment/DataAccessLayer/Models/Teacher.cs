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
    public class Teacher : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public string TeacherCode { get; set; }
        public string Degree { get; set; }

        public string? Status { get; set; }
    }

}
