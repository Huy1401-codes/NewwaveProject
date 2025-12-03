using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Messages.Admin
{
    public static class SubjectMessages
    {
        public const string NotFound = "The subject does not exist with {Id}";
        public const string CreateSuccess = "Create a successful {Name} course.";
        public const string CreateFail = "Create {Name} fail.";
        public const string UpdateSuccess = "Update a successful {Name} course";
        public const string UpdateFail = "Update {Name} fail.";
        public const string DeleteSuccess = "Delete a successful {Name} course.";
        public const string DeleteFail = "Delete {Name} Fail.";
        public const string DuplicateName = "The course {Name} already exists.";
        public const string ErrorPaging = "Error while paging subjects.";
        public const string ErrorFail = "Error while getting subject {Id}.";
        public const string ErrorGetAll = "Error while getting all subjects.";
        public const string ErrorGetAllName = "Error while getting subject names.";
    }
}
