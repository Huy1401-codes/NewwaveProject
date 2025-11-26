namespace BusinessLogicLayer.DTOs
{
    public class StudentScheduleDto
    {
        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public string TeacherName { get; set; }
        public int DayOfWeek { get; set; } 
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Room { get; set; }
    }

}
