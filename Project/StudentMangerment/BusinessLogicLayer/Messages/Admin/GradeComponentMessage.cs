using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Messages.Admin
{
    public static class GradeComponentMessage
    {
        public const string CreateSuccess = "Create class successfully";
        public const string UpdateSuccess = "Update class successfully";
        public const string DeleteSuccess = "Delete class successfully";
        public const string GetByIdSuccess = "Get class by id successfully";
        public const string GetAllSuccess = "Get all classes successfully";

        public const string CreateError = "Error while creating GradeComponent for Subject Id {SubjectId}";
        public const string UpdateError = "Error while updating GradeComponent Id {Id}";
        public const string NotFound = "GradeComponent Id {Id} was not found";
        public const string DeleteError = "Error while deleting GradeComponent Id {Id}";
        public const string GetBySubjectError = "Error while getting GradeComponents for Subject Id {SubjectId}";
    }
}
