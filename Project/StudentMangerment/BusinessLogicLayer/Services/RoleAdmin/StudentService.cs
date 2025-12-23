using BusinessLogicLayer.DTOs.ManagerStudent;
using BusinessLogicLayer.Messages;
using BusinessLogicLayer.Messages.Admin;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;

namespace BusinessLogicLayer.Services.RoleAdmin
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<StudentService> _logger;

        public StudentService(IUnitOfWork unitOfWork, ILogger<StudentService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #region Get By Id
        public async Task<Student?> GetByIdAsync(int id)
        {
            try
            {
                return await _unitOfWork.Students.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, StudentMessages.GetByIdError, id);
                throw;
            }
        }
        #endregion

        #region Paged List
        public async Task<(IEnumerable<Student> Data, int TotalCount)> GetPagedAsync(int page, int pageSize, string search)
        {
            try
            {
                return await _unitOfWork.Students.GetPagedAsync(page, pageSize, search);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, StudentMessages.GetPagedError);
                throw;
            }
        }
        #endregion

        #region Create
        public async Task<(bool Success, string ErrorMessage)> CreateAsync(CreateStudentDto dto)
        {
            try
            {
                var users = await _unitOfWork.Users.GetAllAsync();
                var user = users.FirstOrDefault(u => u.Id == dto.UserId && u.UserRoles.Any(ur => ur.Role.Name == "Student"));

                if (user == null)
                    return (false, StudentMessages.InvalidUser);

                var existedStudents = await _unitOfWork.Students.GetAllAsync();

                if (existedStudents.Any(s => s.UserId == dto.UserId))
                    return (false, StudentMessages.UserAlreadyStudent);

                if (existedStudents.Any(s => s.StudentCode.Equals(dto.StudentCode, StringComparison.OrdinalIgnoreCase)))
                    return (false, StudentMessages.StudentCodeExists);

                var student = new Student
                {
                    UserId = dto.UserId!.Value,
                    StudentCode = dto.StudentCode,
                    BirthDate = dto.BirthDate!.Value,
                    Gender = dto.Gender
                };

                await _unitOfWork.Students.AddAsync(student);
                await _unitOfWork.SaveAsync();

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, StudentMessages.CreateError, dto.UserId);
                throw;
            }
        }
        #endregion

        #region Update
        public async Task<bool> UpdateAsync(Student student)
        {
            try
            {
                var existing = await _unitOfWork.Students.GetByIdAsync(student.Id);
                if (existing == null)
                {
                    _logger.LogWarning(StudentMessages.StudentNotFound, student.Id);
                    return false;
                }

                existing.Gender = student.Gender;
                existing.BirthDate = student.BirthDate;

                await _unitOfWork.Students.UpdateAsync(existing);
                await _unitOfWork.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, StudentMessages.UpdateError, student.Id);
                throw;
            }
        }
        #endregion

        #region Soft Delete
        public async Task<bool> SoftDeleteAsync(int id)
        {
            try
            {
                var student = await _unitOfWork.Students.GetByIdAsync(id);
                if (student == null)
                {
                    _logger.LogWarning(StudentMessages.StudentNotFound, id);
                    return false;
                }

                await _unitOfWork.Users.SoftDeleteAsync(id);
                await _unitOfWork.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, StudentMessages.DeleteError, id);
                throw;
            }
        }
        #endregion

        #region Get All
        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            try
            {
                return await _unitOfWork.Students.GetAllNameAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, StudentMessages.GetAllError);
                throw;
            }
        }
        #endregion

        #region Get Available Student Users
        public async Task<IEnumerable<UserDropdownDto>> GetAvailableStudentUsersAsync(string search = null)
        {
            try
            {
                var users = await _unitOfWork.Users.GetAllAsync();
                var students = await _unitOfWork.Students.GetAllAsync();

                var available = users.Where(u => u.UserRoles.Any(ur => ur.Role.Name == "Student") && !students.Any(s => s.UserId == u.Id));

                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.Trim().ToLower();
                    available = available.Where(u =>
                        (u.FullName?.ToLower().Contains(search) ?? false) ||
                        (u.Email?.ToLower().Contains(search) ?? false) ||
                        (u.Phone?.Contains(search) ?? false)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, StudentMessages.GetAvailableUsersError);
                throw;
            }
        }
        #endregion

        public async Task<Student?> GetByStudentCodeAsync(string studentCode)
        {
            return await _unitOfWork.Students.GetByStudentCodeAsync(studentCode);

        }
        public async Task<List<int>> GetStudentIdsFromExcelAsync(IFormFile file)
        {
            var result = new HashSet<int>(); 

            using var package = new ExcelPackage(file.OpenReadStream());
            var sheet = package.Workbook.Worksheets[0];

            for (int row = 2; row <= sheet.Dimension.End.Row; row++)
            {
                if (int.TryParse(sheet.Cells[row, 1].Text, out int studentId))
                {
                    var exists = await _unitOfWork.Students.GetByIdAsync(studentId);
                    if (exists != null)
                        result.Add(studentId);
                }
            }

            return result.ToList();
        }


    }
}
