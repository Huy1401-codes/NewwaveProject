using BusinessLogicLayer.DTOs.ManagerStudent;
using BusinessLogicLayer.DTOs.ManagerTeacher;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
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

        public TeacherService(ITeacherRepository teacherRepo, IUserRepository userRepo)
        {
            _teacherRepo = teacherRepo;
            _userRepo = userRepo;
        }

        public async Task<Teacher> GetByIdAsync(int id)
        {
            return await _teacherRepo.GetByIdAsync(id);
        }

        public async Task<(IEnumerable<Teacher> Data, int TotalCount)> GetPagedAsync(int page, int pageSize, string search)
        {
            return await _teacherRepo.GetPagedAsync(page, pageSize, search);
        }

        public async Task<(bool Success, string ErrorMessage)> CreateAsync(CreateTeacherDto dto)
        {
            // 1. Kiểm tra User hợp lệ
            var user = (await _teacherRepo.GetUsersByRoleAsync("Teacher"))
                .FirstOrDefault(u => u.UserId == dto.UserId);
            if (user == null)
                return (false, "User không hợp lệ hoặc chưa có role Teacher.");

            // 2. Kiểm tra User đã là Student chưa
            var existedStudents = await _teacherRepo.GetAllAsync();
            if (existedStudents.Any(s => s.UserId == dto.UserId))
                return (false, "User này đã là Teacher.");

            // 3. Kiểm tra StudentCode trùng
            if (existedStudents.Any(s => s.TeacherCode.Equals(dto.TeacherCode, StringComparison.OrdinalIgnoreCase)))
                return (false, "TeacherCode đã tồn tại, vui lòng chọn mã khác.");

            // 4. Tạo Student
            var teach = new Teacher
            {
                UserId = (int)dto.UserId,
                TeacherCode = dto.TeacherCode,            
                Degree = dto.Degree,
            };

            await _teacherRepo.AddAsync(teach);
            await _teacherRepo.SaveAsync();

            return (true, string.Empty);
        }

        public async Task<bool> UpdateAsync(Teacher teacher)
        {
            var existing = await _teacherRepo.GetByIdAsync(teacher.TeacherId);
            if (existing == null) return false;

            existing.Degree = teacher.Degree;

            await _teacherRepo.UpdateAsync(existing);
            await _teacherRepo.SaveAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var teacher = await _teacherRepo.GetByIdAsync(id);
            if (teacher == null) return false;

            await _teacherRepo.SoftDeleteAsync(id);
            await _teacherRepo.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<Teacher>> GetAllAsync()
        {
            return await _teacherRepo.GetAllNameAsync();
        }

        public async Task<IEnumerable<UserDropdownDto>> GetAvailableTeacherUsersAsync(string search = null)
        {
            // Lấy danh sách User có role Teacher
            var users = await _teacherRepo.GetUsersByRoleAsync("Teacher");

            // Lấy các Teacher đã tạo
            var teachers = await _teacherRepo.GetAllAsync();

            // Chỉ lấy User chưa có trong bảng Teacher
            var available = users
                .Where(u => !teachers.Any(s => s.UserId == u.UserId));

            // Nếu có search thì lọc
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();

                available = available.Where(u =>
                    (!string.IsNullOrWhiteSpace(u.FullName) && u.FullName.Trim().ToLower().Contains(search)) ||
                    (!string.IsNullOrWhiteSpace(u.Email) && u.Email.Trim().ToLower().Contains(search)) ||
                    (!string.IsNullOrWhiteSpace(u.Phone) && u.Phone.Trim().Contains(search))
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
