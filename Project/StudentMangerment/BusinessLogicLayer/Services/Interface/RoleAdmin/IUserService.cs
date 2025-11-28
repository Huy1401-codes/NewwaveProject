using BusinessLogicLayer.DTOs.Admin;
using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace BusinessLogicLayer.Services.Interface.RoleAdmin
{
    public interface IUserService
    {
        /// <summary>
        /// Lấy danh sách người dùng đã phân trang + lọc + search
        /// </summary>
        Task<(IEnumerable<UserListDto> Users, int Total)> GetPagedUsersAsync(
            string search, int pageIndex, int pageSize, int? roleId, bool? status);

        Task<(IEnumerable<UserListDto> Users, int Total)> GetRestoreUsersAsync(
          string search, int pageIndex, int pageSize, int? roleId);


        /// <summary>
        /// Lấy chi tiết 1 user (dùng DTO)
        /// </summary>
        Task<UserDetailDto> GetByIdAsync(int id);

        /// <summary>
        /// Tạo user mới (bao gồm gán nhiều role)
        /// </summary>
        Task<(bool Success, List<string> Errors)> AddAsync(UserCreateDto dto);

        /// <summary>
        /// Cập nhật user + cập nhật vai trò
        /// </summary>
        Task<(bool Success, List<string> Errors)> UpdateAsync(UserUpdateDto dto);


        /// <summary>
        /// Cấp lại mật khẩu
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        Task<bool> ResetPasswordAsync(int userId, string newPassword);
        /// <summary>
        /// Xóa mềm user
        /// </summary>
        Task DeleteAsync(int id);

        Task<List<Role>> GetAllAsync();

    }

}
