using BusinessLogicLayer.DTOs.Admin;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Interface.RoleAdmin
{

    public interface ISemesterService
    {
        Task<(IEnumerable<SemesterDto> Semesters, int Total)> GetPagedSemestersAsync(string search, int pageIndex, int pageSize);
        Task<SemesterDto> GetByIdAsync(int id);
        Task AddAsync(SemesterCreateDto dto);
        Task UpdateAsync(SemesterDto dto);

        Task<IEnumerable<Semester>> GetAllAsync();

    }
}
