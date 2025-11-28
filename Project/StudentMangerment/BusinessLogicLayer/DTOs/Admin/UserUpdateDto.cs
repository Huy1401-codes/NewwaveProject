using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.DTOs.Admin
{
    public class UserUpdateDto
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Họ và tên không được để trống.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải gồm 10 chữ số.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn trạng thái cho tài khoản.")]
        public bool? IsStatus { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn vai trò cho tài khoản.")]
        public int RoleId { get; set; }
    }

}
