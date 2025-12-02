using BusinessLogicLayer.DTOs.ManagerStudent;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;

namespace BusinessLogicLayer.Services.RoleAdmin
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly IUserRepository _userRepo;

        public StudentService(IStudentRepository studentRepo, IUserRepository userRepo)
        {
            _studentRepo = studentRepo;
            _userRepo = userRepo;
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            return await _studentRepo.GetByIdAsync(id);
        }

        public async Task<(IEnumerable<Student> Data, int TotalCount)> GetPagedAsync(int page, int pageSize, string search)
        {
            return await _studentRepo.GetPagedAsync(page, pageSize, search);
        }

        public async Task<(bool Success, string ErrorMessage)> CreateAsync(CreateStudentDto dto)
        {
            var user = (await _studentRepo.GetUsersByRoleAsync("Student"))
                .FirstOrDefault(u => u.UserId == dto.UserId);

            if (user == null)
                return (false, "User không hợp lệ hoặc chưa có role Student.");

            var existedStudents = await _studentRepo.GetAllAsync();
            if (existedStudents.Any(s => s.UserId == dto.UserId))
                return (false, "User này đã là Student.");

            if (existedStudents.Any(s => s.StudentCode.Equals(dto.StudentCode, StringComparison.OrdinalIgnoreCase)))
                return (false, "StudentCode đã tồn tại, vui lòng chọn mã khác.");

            var student = new Student
            {
                UserId = (int)dto.UserId,
                StudentCode = dto.StudentCode,
                BirthDate = (DateTime)dto.BirthDate,
                Gender = dto.Gender
            };

            await _studentRepo.AddAsync(student);
            await _studentRepo.SaveAsync();

            return (true, string.Empty);
        }


        public async Task<bool> UpdateAsync(Student student)
        {
            var existing = await _studentRepo.GetByIdAsync(student.StudentId);
            if (existing == null) return false;

            existing.Gender = student.Gender;
            existing.BirthDate = student.BirthDate;

            await _studentRepo.UpdateAsync(existing);
            await _studentRepo.SaveAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var student = await _studentRepo.GetByIdAsync(id);
            if (student == null) return false;

            await _studentRepo.SoftDeleteAsync(id);
            await _studentRepo.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _studentRepo.GetAllAsync();
        }

        public async Task<IEnumerable<UserDropdownDto>> GetAvailableStudentUsersAsync(string search = null)
        {
            var users = await _studentRepo.GetUsersByRoleAsync("Student");

            var students = await _studentRepo.GetAllAsync();

            var available = users
                .Where(u => !students.Any(s => s.UserId == u.UserId));

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();

                available = available.Where(u =>
                    (u.FullName != null && u.FullName.ToLower().Contains(search)) ||
                    (u.Email != null && u.Email.ToLower().Contains(search)) ||
                    (u.Phone != null && u.Phone.Contains(search))
                );
            }

            return available
                .Select(u => new UserDropdownDto
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
