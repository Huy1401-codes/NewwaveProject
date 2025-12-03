using BusinessLogicLayer.DTOs.ManagerStudent;
using BusinessLogicLayer.DTOs.ManagerTeacher;
using BusinessLogicLayer.Messages.Admin;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.RoleAdmin
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepo;
        private readonly IUserRepository _userRepo;
        private readonly ILogger<TeacherService> _logger;

        public TeacherService(
            ITeacherRepository teacherRepo,
            IUserRepository userRepo,
            ILogger<TeacherService> logger)
        {
            _teacherRepo = teacherRepo;
            _userRepo = userRepo;
            _logger = logger;
        }

        public async Task<Teacher> GetByIdAsync(int id)
        {
            var teacher = await _teacherRepo.GetByIdAsync(id);

            if (teacher == null)
                _logger.LogWarning(TeacherMessages.NotFound, id);

            return teacher;
        }

        public async Task<(IEnumerable<Teacher> Data, int TotalCount)> GetPagedAsync(int page, int pageSize, string search)
        {
            return await _teacherRepo.GetPagedAsync(page, pageSize, search);
        }

        public async Task<(bool Success, string ErrorMessage)> CreateAsync(CreateTeacherDto dto)
        {
            try
            {
                var teacherUsers = await _teacherRepo.GetUsersByRoleAsync("Teacher");
                var user = teacherUsers.FirstOrDefault(u => u.UserId == dto.UserId);

                if (user == null)
                {
                    _logger.LogWarning(TeacherMessages.InvalidUser, dto.UserId);
                    return (false, TeacherMessages.InvalidUser);
                }

                var existedTeachers = await _teacherRepo.GetAllAsync();
                if (existedTeachers.Any(t => t.UserId == dto.UserId))
                {
                    _logger.LogWarning(TeacherMessages.DuplicateUser, dto.UserId);
                    return (false, TeacherMessages.DuplicateUser);
                }

                if (existedTeachers.Any(t =>
                        t.TeacherCode.Equals(dto.TeacherCode, StringComparison.OrdinalIgnoreCase)))
                {
                    _logger.LogWarning(TeacherMessages.DuplicateCode, dto.TeacherCode);
                    return (false, TeacherMessages.DuplicateCode);
                }

                var teacher = new Teacher
                {
                    UserId = dto.UserId.Value,
                    TeacherCode = dto.TeacherCode,
                    Degree = dto.Degree,
                };

                await _teacherRepo.AddAsync(teacher);
                await _teacherRepo.SaveAsync();

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
                var existing = await _teacherRepo.GetByIdAsync(teacher.TeacherId);
                if (existing == null)
                {
                    _logger.LogWarning(TeacherMessages.NotFound , teacher.TeacherId);
                    return false;
                }

                existing.Degree = teacher.Degree;

                await _teacherRepo.UpdateAsync(existing);
                await _teacherRepo.SaveAsync();

                _logger.LogInformation(TeacherMessages.UpdateSuccess, teacher.TeacherId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, TeacherMessages.UpdateFail, teacher.TeacherId);
                return false;
            }
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            try
            {
                var teacher = await _teacherRepo.GetByIdAsync(id);
                if (teacher == null)
                {
                    _logger.LogWarning(TeacherMessages.NotFound, id);
                    return false;
                }

                await _teacherRepo.SoftDeleteAsync(id);
                await _teacherRepo.SaveAsync();

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
                return await _teacherRepo.GetAllNameAsync();
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, TeacherMessages.Fail);
                throw;
            }
        }

        public async Task<IEnumerable<UserDropdownDto>> GetAvailableTeacherUsersAsync(string search = null)
        {
            var users = await _teacherRepo.GetUsersByRoleAsync("Teacher");
            var teachers = await _teacherRepo.GetAllAsync();

            var available = users
                .Where(u => !teachers.Any(t => t.UserId == u.UserId));

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
                UserId = u.UserId,
                FullName = u.FullName,
                Email = u.Email,
                Phone = u.Phone
            })
            .ToList();
        }
    }
}
