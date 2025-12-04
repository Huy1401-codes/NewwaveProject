using DataAccessLayer.Models;

namespace BusinessLogicLayer.DTOs.Results
{
    public class LoginResult
    {
        public User User { get; set; }
        public string ErrorMessage { get; set; }
    }
}
