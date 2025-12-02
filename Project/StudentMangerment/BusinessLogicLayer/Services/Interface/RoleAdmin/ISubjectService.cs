using BusinessLogicLayer.DTOs;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Interface.RoleAdmin
{
    public interface ISubjectService
    {
        Task<(IEnumerable<SubjectDto> data, int totalItems)> GetPagedSubjectsAsync(
            string? search, int page, int pageSize);

        Task<Subject?> GetByIdAsync(int id);
        Task<bool> CreateAsync(Subject subject);
        Task<bool> UpdateAsync(Subject subject);
        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<Subject>> GetAllNameAsync();

    }

}
