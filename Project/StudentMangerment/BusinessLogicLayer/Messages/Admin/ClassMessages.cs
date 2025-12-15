using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Messages.Admin
{
    public static class ClassMessages
    {
        public const string CreateError = "Error while creating Class '{ClassName}'";
        public const string UpdateError = "Error while updating Class Id {ClassId}";
        public const string ClassNotFound = "Class with Id {ClassId} not found";
        public const string DeleteError = "Error while deleting Class Id {ClassId}";
        public const string GetByIdError = "Error while getting Class Id {ClassId}";
        public const string GetAllError = "Error while getting all Classes";

        public const string CreateSuccess = "Create class successfully";
        public const string UpdateSuccess = "Update class successfully";
        public const string DeleteSuccess = "Delete class successfully";
        public const string GetByIdSuccess = "Get class by id successfully";
        public const string GetAllSuccess = "Get all classes successfully";
    }
}
