using BusinessLogicLayer.DTOs;
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
    public class GradeComponentService : IGradeComponentService
    {
        private readonly IGradeComponentRepository _repo;

        public GradeComponentService(IGradeComponentRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// danh sách điểm thành phần
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        public Task<IEnumerable<GradeComponent>> GetComponentsOfSubject(int subjectId)
            => _repo.GetBySubjectAsync(subjectId);

        /// <summary>
        /// Thêm điểm thành phần
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task AddComponent(CreateGradeComponentDto dto)
        {
            var entity = new GradeComponent
            {
                SubjectId = dto.SubjectId,
                ComponentName = dto.ComponentName,
                Weight = dto.Weight,
                IsDeleted = false
            };

            await _repo.AddAsync(entity);
        }

        /// <summary>
        /// chỉnh sửa thành phần
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task UpdateComponent(int id, UpdateGradeComponentDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return;

            entity.ComponentName = dto.ComponentName;
            entity.Weight = dto.Weight;

            await _repo.UpdateAsync(entity);
        }

        public Task DeleteComponent(int id)
            => _repo.SoftDeleteAsync(id);
    }

}
