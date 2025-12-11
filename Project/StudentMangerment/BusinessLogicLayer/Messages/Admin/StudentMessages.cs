namespace BusinessLogicLayer.Messages.Admin
{
    public static class StudentMessages
    {
        public const string GetByIdError = "Error while getting Student Id {Id}.";
        public const string GetPagedError = "Error while getting paged Students.";
        public const string CreateError = "Error while creating Student for User Id {UserId}.";
        public const string UpdateError = "Error while updating Student Id {Id}.";
        public const string DeleteError = "Error while deleting Student Id {Id}.";
        public const string GetAllError = "Error while retrieving all Students.";
        public const string GetAvailableUsersError = "Error while retrieving available Student users.";
        public const string InvalidUser = "User is invalid or does not have Student role.";
        public const string UserAlreadyStudent = "This user is already a Student.";
        public const string StudentCodeExists = "StudentCode already exists, please choose another code.";
        public const string StudentNotFound = "Student {Id} not found.";
    }
}
