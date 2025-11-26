using BusinessLogicLayer.DTOs;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.Interface.RoleAdmin
{
    public interface IGradeComponentService
    {
        Task<IEnumerable<GradeComponent>> GetComponentsOfSubject(int subjectId);
        Task AddComponent(CreateGradeComponentDto dto);
        Task UpdateComponent(int id, UpdateGradeComponentDto dto);
        Task DeleteComponent(int id);
    }

}
