using BusinessLogicLayer.DTOs.ManagerStudent;
using BusinessLogicLayer.Messages;
using BusinessLogicLayer.Messages.Admin;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.Services.RoleAdmin
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly IUserRepository _userRepo;
        private readonly ILogger<StudentService> _logger;

        public StudentService(
            IStudentRepository studentRepo,
            IUserRepository userRepo,
            ILogger<StudentService> logger)
        {
            _studentRepo = studentRepo;
            _userRepo = userRepo;
            _logger = logger;
        }

        /// <summary>
        /// Get Student by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Student> GetByIdAsync(int id)
        {
            try
            {
                return await _studentRepo.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, StudentMessages.GetByIdError, id);
                throw;
            }
        }

        /// <summary>
        /// Get All student
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="search"></param>
        /// <returns></returns>

        public async Task<(IEnumerable<Student> Data, int TotalCount)> GetPagedAsync(int page, int pageSize, string search)
        {
            try
            {
                return await _studentRepo.GetPagedAsync(page, pageSize, search);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, StudentMessages.GetPagedError);
                throw;
            }
        }

        /// <summary>
        /// Add student
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<(bool Success, string ErrorMessage)> CreateAsync(CreateStudentDto dto)
        {
            try
            {
                var user = (await _studentRepo.GetUsersByRoleAsync("Student"))
                    .FirstOrDefault(u => u.Id == dto.UserId);

                if (user == null)
                    return (false, StudentMessages.InvalidUser);

                var existedStudents = await _studentRepo.GetAllAsync();

                if (existedStudents.Any(s => s.UserId == dto.UserId))
                    return (false, StudentMessages.UserAlreadyStudent);

                if (existedStudents.Any(s =>
                        s.StudentCode.Equals(dto.StudentCode, StringComparison.OrdinalIgnoreCase)))
                    return (false, StudentMessages.StudentCodeExists);

                var student = new Student
                {
                    UserId = dto.UserId!.Value,
                    StudentCode = dto.StudentCode,
                    BirthDate = dto.BirthDate!.Value,
                    Gender = dto.Gender
                };

                await _studentRepo.AddAsync(student);
                await _studentRepo.SaveAsync();

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, StudentMessages.CreateError, dto.UserId);
                throw;
            }
        }


        public async Task<bool> UpdateAsync(Student student)
        {
            try
            {
                var existing = await _studentRepo.GetByIdAsync(student.Id);
                if (existing == null)
                {
                    _logger.LogWarning(StudentMessages.StudentNotFound, student.Id);
                    return false;
                }

                existing.Gender = student.Gender;
                existing.BirthDate = student.BirthDate;

                await _studentRepo.UpdateAsync(existing);
                await _studentRepo.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, StudentMessages.UpdateError, student.Id);
                throw;
            }
        }


        public async Task<bool> SoftDeleteAsync(int id)
        {
            try
            {
                var student = await _studentRepo.GetByIdAsync(id);
                if (student == null)
                {
                    _logger.LogWarning(StudentMessages.StudentNotFound, id);
                    return false;
                }

                await _studentRepo.SoftDeleteAsync(id);
                await _studentRepo.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, StudentMessages.DeleteError, id);
                throw;
            }
        }


        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            try
            {
                return await _studentRepo.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, StudentMessages.GetAllError);
                throw;
            }
        }


        public async Task<IEnumerable<UserDropdownDto>> GetAvailableStudentUsersAsync(string search = null)
        {
            try
            {
                var users = await _studentRepo.GetUsersByRoleAsync("Student");

                var students = await _studentRepo.GetAllAsync();

                var available = users.Where(u => !students.Any(s => s.UserId == u.Id));

                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.Trim().ToLower();

                    available = available.Where(u =>
                        (u.FullName?.ToLower().Contains(search) ?? false) ||
                        (u.Email?.ToLower().Contains(search) ?? false) ||
                        (u.Phone?.Contains(search) ?? false)
                    );
                }

                return available
                    .Select(u => new UserDropdownDto
                    {
                        UserId = u.Id,
                        FullName = u.FullName,
                        Email = u.Email,
                        Phone = u.Phone
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, StudentMessages.GetAvailableUsersError);
                throw;
            }
        }
    }
}
