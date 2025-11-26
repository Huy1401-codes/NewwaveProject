using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.RoleAdmin
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepo;
        public ClassService(IClassRepository classRepo)
        {
            _classRepo = classRepo;
        }

        /// <summary>
        /// Danh sách class kết  hợp các điều kiện
        /// </summary>
        /// <param name="search">tìm kiếm theo tên</param>
        /// <param name="semesterId">Lọc theo kì học</param>
        /// <param name="subjectId">Lọc theo môn học</param>
        /// <param name="teacherId">Lọc theo giáo viên</param>
        /// <param name="page">Phân trang</param>
        /// <param name="pageSize">Số lượng hiển thị trên 1 trang</param>
        /// <returns></returns>
        public async Task<(IEnumerable<Class> data, int totalItems)> GetPagedClassesAsync(
              string? search, int? semesterId, int? subjectId, int? teacherId,
              int page, int pageSize)
        {
            var query = _classRepo.GetAllQueryable();

            // Search by class name
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                query = query.Where(c => c.ClassName.ToLower().Contains(search));
            }

            // Filter by semester
            if (semesterId.HasValue)
            {
                query = query.Where(c => c.SemesterId == semesterId.Value);
            }

            // Filter by subject
            if (subjectId.HasValue)
            {
                query = query.Where(c => c.SubjectId == subjectId.Value);
            }

            // Filter by teacher
            if (teacherId.HasValue)
            {
                query = query.Where(c => c.TeacherId == teacherId.Value);
            }

            int totalItems = await query.CountAsync();

            var data = await query
                .OrderBy(c => c.ClassName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, totalItems);
        }

        public async Task<Class?> GetByIdAsync(int id)
        {
            return await _classRepo.GetByIdAsync(id);
        }

        public async Task<bool> CreateAsync(Class cls)
        {
            await _classRepo.AddAsync(cls);
            await _classRepo.SaveAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Class cls)
        {
            var exist = await _classRepo.GetByIdAsync(cls.ClassId);
            if (exist == null) return false;

            exist.ClassName = cls.ClassName;
            exist.SubjectId = cls.SubjectId;
            exist.SemesterId = cls.SemesterId;
            exist.TeacherId = cls.TeacherId;

            await _classRepo.UpdateAsync(exist);
            await _classRepo.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _classRepo.SoftDeleteAsync(id);
            await _classRepo.SaveAsync();
            return true;
        }

        public async Task<List<Class>> GetAllAsync()
        {
            return await _classRepo.GetAllQueryable().ToListAsync();
        }
    }
}
