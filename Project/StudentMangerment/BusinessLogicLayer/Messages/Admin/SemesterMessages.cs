using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Messages.Admin
{
    public static class SemesterMessages
    {
        public const string GetPagedSuccess = " GetPaged Success!";
        public const string CreateSuccess = " Create Semester Success!";
        public const string UpdateSuccess = " Update Semester Success!";

        public const string GetPagedError = "Error while getting paged Semesters.";
        public const string GetByIdError = "Error while getting Semester Id {Id}.";
        public const string AddError = "Error while adding Semester {Name}.";
        public const string UpdateError = "Error while updating Semester Id {Id}.";
        public const string InvalidDate = "StartDate must be earlier than EndDate.";
        public const string GetAll = "Error when getting all semesters.";
        public const string NotFound = "Semester Id {Id} not found.";
        public const string DuplicateInTimeRange = "The semester has existed for this period of time.";
    }
}
