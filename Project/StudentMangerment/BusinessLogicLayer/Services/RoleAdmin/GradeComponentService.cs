using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Messages.Admin;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<GradeComponentService> _logger;

        public GradeComponentService(IGradeComponentRepository repo, ILogger<GradeComponentService> logger)
        {
            _repo = repo;
            _logger = logger;
        }


        /// <summary>
        /// List grade component of Subject
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<GradeComponent>> GetComponentsOfSubject(int subjectId)
        {
            try
            {
                return await _repo.GetBySubjectAsync(subjectId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, GradeComponentMessage.GetBySubjectError, subjectId);
                throw;
            }
        }

        /// <summary>
        /// Add component
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task AddComponent(CreateGradeComponentDto dto)
        {
            try
            {
                var entity = new GradeComponent
                {
                    SubjectId = dto.SubjectId,
                    ComponentName = dto.ComponentName,
                    Weight = dto.Weight,
                    IsDeleted = false
                };
                _logger.LogInformation(GradeComponentMessage.CreateSuccess);
                await _repo.AddAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, GradeComponentMessage.CreateError, dto.SubjectId);
                throw;
            }
        }

        /// <summary>
        /// Update compenent grade
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task UpdateComponent(int id, UpdateGradeComponentDto dto)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);

                if (entity == null)
                {
                    _logger.LogWarning(GradeComponentMessage.NotFound, id);
                    return;
                }

                entity.ComponentName = dto.ComponentName;
                entity.Weight = dto.Weight;

                await _repo.UpdateAsync(entity);
                _logger.LogInformation(GradeComponentMessage.UpdateSuccess);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, GradeComponentMessage.UpdateError, id);
                throw;
            }
        }

        public async Task DeleteComponent(int id)
        {
            try
            {
                await _repo.SoftDeleteAsync(id);
                _logger.LogInformation(GradeComponentMessage.DeleteSuccess);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, GradeComponentMessage.DeleteError, id);
                throw;
            }
        }
    }

}
