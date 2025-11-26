namespace PresentationLayer.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsStatus { get; set; }
        public string RoleName { get; set; } // Tên vai trò
    }
}
