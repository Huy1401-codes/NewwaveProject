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

        public async Task<bool> CreateAsync(Teacher teacher)
        {
            var user = await _userRepo.GetByIdAsync(teacher.UserId);
            if (user == null) return false;

            // Check user đã có vai trò Teacher chưa
            var existed = await _teacherRepo.GetAllAsync();
            if (existed.Any(t => t.UserId == teacher.UserId))
                return false;

            await _teacherRepo.AddAsync(teacher);
            await _teacherRepo.SaveAsync();
            return true;
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
    }

}
