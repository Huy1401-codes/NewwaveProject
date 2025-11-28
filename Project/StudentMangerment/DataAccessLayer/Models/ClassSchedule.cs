using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ClassSchedule
    {
        [Key]
        public int ScheduleId { get; set; }

        [Required]
        public int ClassId { get; set; }
        public Class Class { get; set; }

        [Required]
        public int ClassSemesterId { get; set; }  // thêm FK mới
        public ClassSemester ClassSemester { get; set; } // navigation property
        [Range(1, 7)]
        public int DayOfWeek { get; set; } // 1 = Monday

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        [StringLength(50)]
        public string Room { get; set; }
    }
}
