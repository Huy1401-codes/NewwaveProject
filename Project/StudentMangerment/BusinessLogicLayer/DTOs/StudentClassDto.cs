namespace BusinessLogicLayer.DTOs
{
    public class StudentClassDto
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public string TeacherName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SemesterName { get; set; }
    }

}
