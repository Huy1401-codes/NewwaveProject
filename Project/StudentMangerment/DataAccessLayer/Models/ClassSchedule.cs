using DataAccessLayer.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ClassSchedule : BaseEntity
    {
        public int ClassId { get; set; }
        public Class Class { get; set; }

        public int ClassSemesterId { get; set; }
        public ClassSemester ClassSemester { get; set; }

        public int DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        [StringLength(50)]
        public string Room { get; set; }
    }

}
