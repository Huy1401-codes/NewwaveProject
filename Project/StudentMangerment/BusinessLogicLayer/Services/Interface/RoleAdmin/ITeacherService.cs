using BusinessLogicLayer.DTOs.ManagerStudent;
using BusinessLogicLayer.DTOs.ManagerTeacher;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.Interface.RoleAdmin
{
    public interface ITeacherService
    {
        Task<Teacher> GetByIdAsync(int id);
        Task<(IEnumerable<Teacher> Data, int TotalCount)> GetPagedAsync(int page, int pageSize, string search);
        Task<(bool Success, string ErrorMessage)> CreateAsync(CreateTeacherDto dto);
        Task<bool> UpdateAsync(Teacher teacher);
        Task<bool> SoftDeleteAsync(int id);

        Task<IEnumerable<Teacher>> GetAllAsync();

        Task<IEnumerable<UserDropdownDto>> GetAvailableTeacherUsersAsync(string search = null);

        Task<int?> GetTeacherIdByUserIdAsync(int userId);

    }

}
