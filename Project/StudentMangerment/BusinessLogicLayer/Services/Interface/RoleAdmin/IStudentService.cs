using BusinessLogicLayer.DTOs.ManagerStudent;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;

namespace BusinessLogicLayer.Services.Interface.RoleAdmin
{
    public interface IStudentService
    {
        Task<Student> GetByIdAsync(int id);
        Task<(IEnumerable<Student> Data, int TotalCount)> GetPagedAsync(int page, int pageSize, string search);

        Task<(bool Success, string ErrorMessage)> CreateAsync(CreateStudentDto dto);
        
        Task<bool> UpdateAsync(Student student);
        Task<bool> SoftDeleteAsync(int id);

        Task<IEnumerable<Student>> GetAllAsync();

        Task<IEnumerable<UserDropdownDto>> GetAvailableStudentUsersAsync(string search = null);

        Task<Student?> GetByStudentCodeAsync(string studentCode);
        Task<List<int>> GetStudentIdsFromExcelAsync(IFormFile file);


    }
}
