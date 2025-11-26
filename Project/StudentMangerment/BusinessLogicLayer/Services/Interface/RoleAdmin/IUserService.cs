using BusinessLogicLayer.DTOs.Admin;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.Interface.RoleAdmin
{
    public interface IUserService
    {
        /// <summary>
        /// Lấy danh sách người dùng đã phân trang + lọc + search
        /// </summary>
        Task<(IEnumerable<UserListDto> Users, int Total)> GetPagedUsersAsync(
            string search, int pageIndex, int pageSize, int? roleId, bool? status);

        /// <summary>
        /// Lấy chi tiết 1 user (dùng DTO)
        /// </summary>
        Task<UserDetailDto> GetByIdAsync(int id);

        /// <summary>
        /// Tạo user mới (bao gồm gán nhiều role)
        /// </summary>
        Task AddAsync(UserCreateDto dto);

        /// <summary>
        /// Cập nhật user + cập nhật vai trò
        /// </summary>
        Task UpdateAsync(UserUpdateDto dto);

        /// <summary>
        /// Xóa mềm user
        /// </summary>
        Task DeleteAsync(int id);

        Task<List<Role>> GetAllAsync();
    }

}
