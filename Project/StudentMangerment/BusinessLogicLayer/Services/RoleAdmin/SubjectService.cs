using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.ManagerTeacher;
using BusinessLogicLayer.Messages.Admin;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.Services.RoleAdmin
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepo;
        private readonly ILogger<SubjectService> _logger;

        public SubjectService(ISubjectRepository subjectRepo, ILogger<SubjectService> logger)
        {
            _subjectRepo = subjectRepo;
            _logger = logger;
        }

        /// <summary>
        /// Get list Subject
        /// </summary>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<SubjectDto> data, int totalItems)> GetPagedSubjectsAsync(
              string? search, int page, int pageSize)
        {
            try
            {
                var query = _subjectRepo.GetAllQueryable();

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
                _logger.LogError(ex,SubjectMessages.ErrorPaging);
                throw;
            }
        }


        public async Task<Subject?> GetByIdAsync(int id)
        {
            try
            {
                var subject = await _subjectRepo.GetByIdAsync(id);

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


        public async Task<bool> CreateAsync(Subject subject)
        {
            try
            {
                bool exists = await _subjectRepo
                    .AnyAsync(s => s.Name.ToLower() == subject.Name.ToLower());

                if (exists)
                {
                    _logger.LogWarning(SubjectMessages.DuplicateName, subject.Name);
                    return false;
                }

                await _subjectRepo.AddAsync(subject);
                await _subjectRepo.SaveAsync();

                _logger.LogInformation(SubjectMessages.CreateSuccess , subject.Name);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, SubjectMessages.CreateFail , subject.Name);
                return false;
            }
        }


        public async Task<bool> UpdateAsync(Subject subject)
        {
            try
            {
                var exist = await _subjectRepo.GetByIdAsync(subject.Id);
                if (exist == null)
                {
                    _logger.LogWarning(SubjectMessages.NotFound , subject.Id);
                    return false;
                }

                bool duplicate = await _subjectRepo
                    .AnyAsync(s =>
                        s.Id != subject.Id &&
                        s.Name.ToLower() == subject.Name.ToLower()
                    );

                if (duplicate)
                {
                    _logger.LogWarning(SubjectMessages.DuplicateName , subject.Name);
                    return false;
                }

                exist.Name = subject.Name;
                exist.Credit = subject.Credit;
                exist.IsStatus = subject.IsStatus;

                await _subjectRepo.UpdateAsync(exist);
                await _subjectRepo.SaveAsync();

                _logger.LogInformation(SubjectMessages.UpdateSuccess, subject.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, SubjectMessages.UpdateFail, subject.Id);
                return false;
            }
        }


        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _subjectRepo.SoftDeleteAsync(id);
                await _subjectRepo.SaveAsync();

                _logger.LogInformation(SubjectMessages.DeleteSuccess , id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, SubjectMessages.DeleteFail , id);
                return false;
            }
        }

        public async Task<List<Subject>> GetAllAsync()
        {
            try
            {
                return await _subjectRepo.GetAllQueryable().ToListAsync();
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
                return await _subjectRepo.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, SubjectMessages.ErrorGetAllName);
                throw;
            }
        }
    }
}
