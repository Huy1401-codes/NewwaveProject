using BusinessLogicLayer.DTOs.Admin.ManagerClass;

namespace BusinessLogicLayer.Services.Interface.RoleAdmin
{
    public interface IClassSemesterService
    {
        Task<bool> CreateAsync(ClassCreateDto dto);
        Task<bool> UpdateAsync(ClassUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<ClassDetailDto> GetByIdAsync(int id);
        Task<List<ClassDetailDto>> GetAllAsync();
    }

}
