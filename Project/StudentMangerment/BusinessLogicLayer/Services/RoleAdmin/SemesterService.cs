using BusinessLogicLayer.DTOs.Admin;
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
    public class SemesterService : ISemesterService
    {
        private readonly ISemesterRepository _semesterRepo;

        public SemesterService(ISemesterRepository semesterRepo)
        {
            _semesterRepo = semesterRepo;
        }

        public async Task<(IEnumerable<SemesterDto> Semesters, int Total)> GetPagedSemestersAsync(string search, int pageIndex, int pageSize)
        {
            var query = _semesterRepo.GetAllQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(s => s.Name.Contains(search));

            int total = await query.CountAsync();

            var data = await query
                .OrderBy(s => s.StartDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new SemesterDto
                {
                    SemesterId = s.SemesterId,
                    Name = s.Name,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate
                })
                .ToListAsync();

            return (data, total);
        }

        public async Task<SemesterDto> GetByIdAsync(int id)
        {
            var s = await _semesterRepo.GetByIdAsync(id);
            if (s == null) return null;

            return new SemesterDto
            {
                SemesterId = s.SemesterId,
                Name = s.Name,
                StartDate = s.StartDate,
                EndDate = s.EndDate
            };
        }

        public async Task AddAsync(SemesterCreateDto dto)
        {
            // Validate ngày
            if (dto.StartDate >= dto.EndDate)
                throw new ArgumentException("Ngày bắt đầu phải nhỏ hơn ngày kết thúc.");

            var semester = new Semester
            {
                Name = dto.Name,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

            await _semesterRepo.AddAsync(semester);
            await _semesterRepo.SaveAsync();
        }

        public async Task UpdateAsync(SemesterDto dto)
        {
            // Validate ngày
            if (dto.StartDate >= dto.EndDate)
                throw new ArgumentException("Ngày bắt đầu phải nhỏ hơn ngày kết thúc.");

            var semester = await _semesterRepo.GetByIdAsync(dto.SemesterId);
            if (semester == null) return;

            semester.Name = dto.Name;
            semester.StartDate = dto.StartDate;
            semester.EndDate = dto.EndDate;

            await _semesterRepo.UpdateAsync(semester);
            await _semesterRepo.SaveAsync();
        }
        public async Task<IEnumerable<Semester>> GetAllAsync()
        {
            return await _semesterRepo.GetAllAsync();
        }


    }
}
