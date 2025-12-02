using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.ManagerTeacher;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.EntityFrameworkCore;

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
        public async Task<(IEnumerable<SubjectDto> data, int totalItems)> GetPagedSubjectsAsync(
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

            // Lấy entity
            var subjects = await query
                .OrderBy(s => s.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Map sang DTO
            var data = subjects.Select(s => new SubjectDto
            {
                SubjectId = s.SubjectId,
                Name = s.Name,
                Credit = s.Credit,
                IsStatus = s.IsStatus,
                GradeComponents = new List<GradeComponentDto>()
            });

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

        public async Task<IEnumerable<Subject>> GetAllNameAsync()
        {
            return await _subjectRepo.GetAllAsync();
        }
    }
}
