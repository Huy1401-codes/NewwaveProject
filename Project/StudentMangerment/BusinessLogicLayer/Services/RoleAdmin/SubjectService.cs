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
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepo;

        public SubjectService(ISubjectRepository subjectRepo)
        {
            _subjectRepo = subjectRepo;
        }

        /// <summary>
        /// Lấy danh sách môn học
        /// </summary>
        /// <param name="search">Tìm theo tên</param>
        /// <param name="page">phân trang</param>
        /// <param name="pageSize">Số lượng hiển thị trên 1 trang</param>
        /// <returns></returns>
        public async Task<(IEnumerable<Subject> data, int totalItems)> GetPagedSubjectsAsync(
            string? search, int page, int pageSize)
        {
            var query = _subjectRepo.GetAllQueryable();

            // Search
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                query = query.Where(s => s.Name.ToLower().Contains(search));
            }

            int totalItems = await query.CountAsync();

            // Pagination
            var data = await query
                .OrderBy(s => s.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, totalItems);
        }

        public async Task<Subject?> GetByIdAsync(int id) =>
            await _subjectRepo.GetByIdAsync(id);

        public async Task<bool> CreateAsync(Subject subject)
        {
            await _subjectRepo.AddAsync(subject);
            await _subjectRepo.SaveAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Subject subject)
        {
            var exist = await _subjectRepo.GetByIdAsync(subject.SubjectId);
            if (exist == null) return false;

            exist.Name = subject.Name;
           // exist.Description = subject.Description;

            await _subjectRepo.UpdateAsync(exist);
            await _subjectRepo.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _subjectRepo.SoftDeleteAsync(id);
            await _subjectRepo.SaveAsync();
            return true;
        }

        public async Task<List<Subject>> GetAllAsync()
        {
            return await _subjectRepo.GetAllQueryable().ToListAsync();
        }
    }
}
