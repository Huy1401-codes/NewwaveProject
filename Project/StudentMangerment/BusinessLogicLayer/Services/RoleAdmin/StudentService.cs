using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<bool> CreateAsync(Student student)
        {
            // Kiểm tra UserId có hợp lệ hay không
            var user = await _userRepo.GetByIdAsync(student.UserId);
            if (user == null) return false;

            // Check user đã là student hay chưa
            var existed = await _studentRepo.GetAllAsync();
            if (existed.Any(s => s.UserId == student.UserId))
                return false;

            await _studentRepo.AddAsync(student);
            await _studentRepo.SaveAsync();
            return true;
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

     
    }
}
