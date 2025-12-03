using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Messages.Admin
{
    public static class TeacherMessages
    {
        public const string NotFound = "No instructor found {Id}.";
        public const string CreateSuccess = "Creating {Code} successful instructors.";
        public const string CreateFail = "Create failed instructors.";
        public const string DuplicateCode = "Instructor {Code} already exists.";
        public const string DuplicateUser = "This user {Id} has been assigned as a lecturer.";
        public const string InvalidUser = "Invalid user {Id} or does not have Teacher role.";
        public const string UpdateSuccess = "Lecturer {Id} update successful.";
        public const string UpdateFail = "Instructor {Id} update failed.";
        public const string DeleteSuccess = "Delete {Id} lecturer successfully.";
        public const string DeleteFail = "Delete {Id} lecturer failed.";
        public const string Fail = "Failed.";
    }
}
