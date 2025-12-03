namespace BusinessLogicLayer.Messages.Student
{
    public static class StudentMessages
    {
        public const string StudentNotFound = "Student {userId} does not exist in the system.";

        public const string ListStudent = "Get the class list for student {StudentId}, Total: {Total}.";

        public const string InvalidClassName = "Class name cannot be empty.";

        public const string InvalidSubjectName = "Subject name cannot be empty.";

        public const string InvalidTeacherName = "Teacher name cannot be empty.";

        public const string GradeNotFound = "Class not found for student {StudentId}.";

        public const string ScheduleNotFound = "Timetable not found for student {StudentId}.";

        public const string ScheduleListInfo = "Get the schedule list for student {StudentId}, Total: {Total}.";

    }
}
