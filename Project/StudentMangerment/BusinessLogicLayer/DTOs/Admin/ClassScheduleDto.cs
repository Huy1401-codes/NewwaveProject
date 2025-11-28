using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs.Admin
{
    public class ClassScheduleDto
    {
        public int ScheduleId { get; set; }
        public int ClassId { get; set; }
        public int DayOfWeek { get; set; }   // 1 = Monday
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Room { get; set; }
    }

}
