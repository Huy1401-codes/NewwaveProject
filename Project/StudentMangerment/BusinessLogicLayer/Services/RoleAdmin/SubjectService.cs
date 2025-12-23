using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.ManagerTeacher;
using BusinessLogicLayer.Messages.Admin;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.Services.RoleAdmin
{
    public class SubjectService : ISubjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SubjectService> _logger;

        public SubjectService(IUnitOfWork unitOfWork, ILogger<SubjectService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #region Get Paged Subjects
        public async Task<(IEnumerable<SubjectDto> data, int totalItems)> GetPagedSubjectsAsync(
            string? search, int page, int pageSize)
        {
            try
            {
                var query = _unitOfWork.Subjects.GetActiveSubjects();

                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    query = query.Where(s => s.Name.ToLower().Contains(search));
                }

                int totalItems = await query.CountAsync();

                var subjects = await query
                    .OrderBy(s => s.Name)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var data = subjects.Select(s => new SubjectDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Credit = s.Credit,
                    IsStatus = s.IsStatus,
                    GradeComponents = new List<GradeComponentDto>()
                });

                return (data, totalItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, SubjectMessages.ErrorPaging);
                throw;
            }
        }
        #endregion

        #region Get By Id
        public async Task<Subject?> GetByIdAsync(int id)
        {
            try
            {
                var subject = await _unitOfWork.Subjects.GetByIdAsync(id);

                if (subject == null)
                    _logger.LogWarning(SubjectMessages.NotFound, id);

                return subject;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, SubjectMessages.ErrorFail, id);
                throw;
            }
        }
        #endregion

        #region Create
        public async Task<bool> CreateAsync(Subject subject)
        {
            try
            {
                bool exists = await _unitOfWork.Subjects
                    .AnyAsync(s => s.Name.ToLower() == subject.Name.ToLower());

                if (exists)
                {
                    _logger.LogWarning(SubjectMessages.DuplicateName, subject.Name);
                    return false;
                }

                await _unitOfWork.Subjects.AddAsync(subject);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation(SubjectMessages.CreateSuccess, subject.Name);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, SubjectMessages.CreateFail, subject.Name);
                return false;
            }
        }
        #endregion

        #region Update
        public async Task<bool> UpdateAsync(Subject subject)
        {
            try
            {
                var exist = await _unitOfWork.Subjects.GetByIdAsync(subject.Id);
                if (exist == null)
                {
                    _logger.LogWarning(SubjectMessages.NotFound, subject.Id);
                    return false;
                }

                bool duplicate = await _unitOfWork.Subjects
                    .AnyAsync(s => s.Id != subject.Id && s.Name.ToLower() == subject.Name.ToLower());

                if (duplicate)
                {
                    _logger.LogWarning(SubjectMessages.DuplicateName, subject.Name);
                    return false;
                }

                exist.Name = subject.Name;
                exist.Credit = subject.Credit;
                exist.IsStatus = subject.IsStatus;

                await _unitOfWork.Subjects.UpdateAsync(exist);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation(SubjectMessages.UpdateSuccess, subject.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, SubjectMessages.UpdateFail, subject.Id);
                return false;
            }
        }
        #endregion

        #region Delete
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _unitOfWork.Subjects.SoftDeleteAsync(id);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation(SubjectMessages.DeleteSuccess, id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, SubjectMessages.DeleteFail, id);
                return false;
            }
        }
        #endregion

        #region Get All
        public async Task<List<Subject>> GetAllAsync()
        {
            try
            {
                return await _unitOfWork.Subjects.GetActiveSubjects().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, SubjectMessages.ErrorGetAll);
                throw;
            }
        }

        public async Task<IEnumerable<Subject>> GetAllNameAsync()
        {
            try
            {
                return await _unitOfWork.Subjects.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, SubjectMessages.ErrorGetAllName);
                throw;
            }
        }
        #endregion
    }
}
