using BusinessLogicLayer.DTOs;

namespace BusinessLogicLayer.Services.Interface.RoleStudent
{
    public interface IStudentService
    {
        Task<List<StudentClassDto>> GetClassesAsync(
            int studentId,
            string? search = null,
            DateTime? from = null,
            DateTime? to = null,
            int page = 1,
            int pageSize = 10);

        Task<List<StudentScheduleDto>> GetStudentSchedulesAsync(
            int studentId,
            int? dayOfWeek = null,
            string? search = null,
            int page = 1,
            int pageSize = 10);
    }
}
