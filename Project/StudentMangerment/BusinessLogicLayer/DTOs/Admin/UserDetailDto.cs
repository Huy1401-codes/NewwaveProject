namespace BusinessLogicLayer.DTOs.Admin
{
    public class UserDetailDto
    {
        public int UserId { get; set; }

        public string Username { get; set; }
        public string FullName { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }

        public bool? IsStatus { get; set; }

        public int RoleIds { get; set; }

        public List<string> RoleNames { get; set; }
    }
}
