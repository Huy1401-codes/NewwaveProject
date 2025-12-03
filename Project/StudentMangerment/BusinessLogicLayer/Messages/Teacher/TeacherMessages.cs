using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Messages.Teacher
{
    public static class TeacherMessages
    {
        public const string ClassNotFound = "Class {ClassId} does not exist.";
        public const string ClassSemesterNotFound = "ClassSemester not found for class {ClassId}.";
        public const string StudentNotFound = "Student {StudentId} does not exist.";
        public const string GradesUpdated = "Updated grades for student {StudentId} in class {ClassId}.";
        public const string GradesImported = "Imported grades from file for class {ClassId}.";
        public const string GradesExported = "Exported grades for class {ClassId}.";
        public const string ClassStatisticsFetched = "Fetched class statistics for class {ClassId}.";
        public const string StudentsFetched = "Fetched {Total} students for class {ClassId}.";
        public const string TeacherClassesFetched = "Fetched {Total} classes for teacher {TeacherId}.";
    }
}
