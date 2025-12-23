using BusinessLogicLayer.DTOs.ManagerTeacher;
using BusinessLogicLayer.DTOs.ManagerStudent;
using BusinessLogicLayer.Messages.Admin;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.Services.RoleAdmin
{
    public class TeacherService : ITeacherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TeacherService> _logger;

        public TeacherService(IUnitOfWork unitOfWork, ILogger<TeacherService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Teacher> GetByIdAsync(int id)
        {
            try
            {
                var teacher = await _unitOfWork.Teachers.GetByIdAsync(id);
                if (teacher == null)
                    _logger.LogWarning(TeacherMessages.NotFound, id);

                return teacher;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, TeacherMessages.Fail);
                throw;
            }
        }

        public async Task<(IEnumerable<Teacher> Data, int TotalCount)> GetPagedAsync(int page, int pageSize, string search)
        {
            return await _unitOfWork.Teachers.GetPagedAsync(page, pageSize, search);
        }

        public async Task<(bool Success, string ErrorMessage)> CreateAsync(CreateTeacherDto dto)
        {
            try
            {
                var teacherUsers = await _unitOfWork.Teachers.GetUsersByRoleAsync("Teacher");
                var user = teacherUsers.FirstOrDefault(u => u.Id == dto.UserId);

                if (user == null)
                    return (false, TeacherMessages.InvalidUser);

                var existedTeachers = await _unitOfWork.Teachers.GetAllAsync();
                if (existedTeachers.Any(t => t.UserId == dto.UserId))
                    return (false, TeacherMessages.DuplicateUser);

                if (existedTeachers.Any(t => t.TeacherCode.Equals(dto.TeacherCode, StringComparison.OrdinalIgnoreCase)))
                    return (false, TeacherMessages.DuplicateCode);

                var teacher = new Teacher
                {
                    UserId = dto.UserId.Value,
                    TeacherCode = dto.TeacherCode,
                    Degree = dto.Degree,
                };

                await _unitOfWork.Teachers.AddAsync(teacher);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation(TeacherMessages.CreateSuccess, dto.TeacherCode);
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, TeacherMessages.CreateFail);
                return (false, TeacherMessages.CreateFail);
            }
        }

        public async Task<bool> UpdateAsync(Teacher teacher)
        {
            try
            {
                var existing = await _unitOfWork.Teachers.GetByIdAsync(teacher.Id);
                if (existing == null)
                    return false;

                existing.Degree = teacher.Degree;

                await _unitOfWork.Teachers.UpdateAsync(existing);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation(TeacherMessages.UpdateSuccess, teacher.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, TeacherMessages.UpdateFail, teacher.Id);
                return false;
            }
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            try
            {
                var teacher = await _unitOfWork.Teachers.GetByIdAsync(id);
                if (teacher == null)
                    return false;

                await _unitOfWork.Teachers.SoftDeleteAsync(id);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation(TeacherMessages.DeleteSuccess, id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, TeacherMessages.DeleteFail, id);
                return false;
            }
        }

        public async Task<IEnumerable<Teacher>> GetAllAsync()
        {
            try
            {
                return await _unitOfWork.Teachers.GetAllNameAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, TeacherMessages.Fail);
                throw;
            }
        }

        public async Task<IEnumerable<UserDropdownDto>> GetAvailableTeacherUsersAsync(string search = null)
        {
            var users = await _unitOfWork.Teachers.GetUsersByRoleAsync("Teacher");
            var teachers = await _unitOfWork.Teachers.GetAllAsync();

            var available = users
                .Where(u => !teachers.Any(t => t.UserId == u.Id));

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                available = available.Where(u =>
                    (!string.IsNullOrWhiteSpace(u.FullName) && u.FullName.ToLower().Contains(search)) ||
                    (!string.IsNullOrWhiteSpace(u.Email) && u.Email.ToLower().Contains(search)) ||
                    (!string.IsNullOrWhiteSpace(u.Phone) && u.Phone.ToLower().Contains(search))
                );
            }

            return available.Select(u => new UserDropdownDto
            {
                UserId = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Phone = u.Phone
            }).ToList();
        }

        public async Task<int?> GetTeacherIdByUserIdAsync(int userId)
        {
            return await _unitOfWork.Teachers.GetTeacherIdByUserIdAsync(userId);
        }
    }
}
