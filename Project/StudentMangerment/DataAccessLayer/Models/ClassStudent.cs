using DataAccessLayer.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ClassStudent : BaseEntity
    {
        public int ClassId { get; set; }
        public Class Class { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public DateTime EnrollDate { get; set; }
    }


}
