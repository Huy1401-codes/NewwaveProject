using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Messages.Admin
{
    public static class ClassStudentMessages
    {
        public const string AddError = "Error while adding Student Id {StudentId} to Class Id {ClassId}";
        public const string RemoveError = "Error while removing Student Id {StudentId} from Class Id {ClassId}";
        public const string GetByClassError = "Error while getting students for Class Id {ClassId}";
        public const string ExistsError = "Error while checking existence of Student Id {StudentId} in Class Id {ClassId}";
    }
}
