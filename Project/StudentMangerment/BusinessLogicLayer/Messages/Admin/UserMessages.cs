namespace BusinessLogicLayer.Messages.Admin
{
    public static class UserMessages
    {
        public const string UsernameRequired = "Username cannot be empty.";
        public const string PasswordRequired = "Password cannot be empty.";
        public const string PasswordInvalid = "Password must be at least 8 characters, including uppercase, lowercase, numbers, and special characters.";
        public const string FullNameRequired = "First and last name cannot be empty.";
        public const string EmailRequired = "Email cannot be empty.";
        public const string EmailInvalid = "Email is not valid.";
        public const string PhoneRequired = "Phone number cannot be empty.";
        public const string PhoneInvalid = "Phone number must be 10 digits.";
        public const string StatusRequired = "Please select a status for the account.";
        public const string RoleRequired = "Please select a role for the account.";

        public const string EmailExists = "Email already exists in the system.";
        public const string PhoneExists = "Phone number already exists in the system.";
        public const string UsernameExists = "Username already exists.";

        
        
        public const string UserNotFound = "User does not exist.";
        public const string UnexpectedError = "An unexpected error occurred.";
        public const string CreateSuccess = "Account created successfully.";
        public const string UpdateSuccess = "Account updated successfully.";
        public const string DeleteSuccess = "Account deleted successfully.";
        public const string Reset = "Password reset for user {userId}.";
        public const string Dulicate = "User creation failed due to duplicate fields.";

    }
}
