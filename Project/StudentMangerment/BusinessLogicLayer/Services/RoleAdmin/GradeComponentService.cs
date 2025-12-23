using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Messages.Admin;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.Services.RoleAdmin
{
    public class GradeComponentService : IGradeComponentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GradeComponentService> _logger;

        public GradeComponentService(IUnitOfWork unitOfWork, ILogger<GradeComponentService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// List grade component of Subject
        /// </summary>
        public async Task<IEnumerable<GradeComponent>> GetComponentsOfSubject(int subjectId)
        {
            try
            {
                return await _unitOfWork.GradeComponents.GetBySubjectAsync(subjectId);
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

                await _unitOfWork.GradeComponents.AddAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation(GradeComponentMessage.CreateSuccess);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, GradeComponentMessage.CreateError, dto.SubjectId);
                throw;
            }
        }

        /// <summary>
        /// Update component grade
        /// </summary>
        public async Task UpdateComponent(int id, UpdateGradeComponentDto dto)
        {
            try
            {
                var entity = await _unitOfWork.GradeComponents.GetByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning(GradeComponentMessage.NotFound, id);
                    return;
                }

                entity.ComponentName = dto.ComponentName;
                entity.Weight = dto.Weight;

                await _unitOfWork.GradeComponents.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation(GradeComponentMessage.UpdateSuccess);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, GradeComponentMessage.UpdateError, id);
                throw;
            }
        }

        /// <summary>
        /// Soft delete component
        /// </summary>
        public async Task DeleteComponent(int id)
        {
            try
            {
                await _unitOfWork.GradeComponents.SoftDeleteAsync(id);
                await _unitOfWork.SaveAsync();

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
