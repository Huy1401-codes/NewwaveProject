using BusinessLogicLayer.DTOs.Admin;
using BusinessLogicLayer.Messages;
using BusinessLogicLayer.Messages.Admin;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.Services.RoleAdmin
{
    public class SemesterService : ISemesterService
    {
        private readonly ISemesterRepository _semesterRepo;
        private readonly ILogger<SemesterService> _logger;

        public SemesterService(ISemesterRepository semesterRepo, ILogger<SemesterService> logger)
        {
            _semesterRepo = semesterRepo;
            _logger = logger;
        }

        /// <summary>
        /// List Semester
        /// </summary>
        /// <param name="search"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<SemesterDto> Semesters, int Total)> GetPagedSemestersAsync(
            string search, int pageIndex, int pageSize)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, SemesterMessages.GetPagedError);
                throw;
            }
        }

        /// <summary>
        /// Get semester by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SemesterDto> GetByIdAsync(int id)
        {
            try
            {
                var s = await _semesterRepo.GetByIdAsync(id);
                if (s == null)
                {
                    _logger.LogWarning(SemesterMessages.NotFound, id);
                    return null;
                }

                return new SemesterDto
                {
                    SemesterId = s.SemesterId,
                    Name = s.Name,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, SemesterMessages.GetByIdError, id);
                throw;
            }
        }

        /// <summary>
        /// Add Sesmester
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task AddAsync(SemesterCreateDto dto)
        {
            try
            {
                if (dto.StartDate >= dto.EndDate)
                {
                    _logger.LogWarning(SemesterMessages.InvalidDate);
                    throw new ArgumentException(SemesterMessages.InvalidDate);
                }
                var lowerName = dto.Name.ToLower();

                var isExist = await _semesterRepo.AnyAsync(s =>
                                 s.StartDate <= dto.EndDate &&
                                 s.EndDate >= dto.StartDate);

                if (isExist)
                {
                    _logger.LogWarning(SemesterMessages.DuplicateInTimeRange);
                    throw new ArgumentException(SemesterMessages.DuplicateInTimeRange);
                }

                var semester = new Semester
                {
                    Name = dto.Name,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate
                };

                await _semesterRepo.AddAsync(semester);
                await _semesterRepo.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, SemesterMessages.AddError, dto.Name);
                throw;
            }
        }

        /// <summary>
        /// Update semester
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task UpdateAsync(SemesterDto dto)
        {
            try
            {
                if (dto.StartDate >= dto.EndDate)
                {
                    _logger.LogWarning(SemesterMessages.InvalidDate);
                    throw new ArgumentException(SemesterMessages.InvalidDate);
                }

                var semester = await _semesterRepo.GetByIdAsync(dto.SemesterId);
                if (semester == null)
                {
                    _logger.LogWarning(SemesterMessages.NotFound, dto.SemesterId);
                    return;
                }

                var lowerName = dto.Name.ToLower();

                var isExist = await _semesterRepo.AnyAsync(s =>
                                       s.SemesterId != dto.SemesterId &&
                                       s.StartDate <= dto.EndDate &&
                                       s.EndDate >= dto.StartDate);

                if (isExist)
                {
                    _logger.LogWarning(SemesterMessages.DuplicateInTimeRange);
                    throw new ArgumentException(SemesterMessages.DuplicateInTimeRange);
                }

                semester.Name = dto.Name;
                semester.StartDate = dto.StartDate;
                semester.EndDate = dto.EndDate;

                await _semesterRepo.UpdateAsync(semester);
                await _semesterRepo.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, SemesterMessages.UpdateError, dto.SemesterId);
                throw;
            }
        }

        /// <summary>
        /// Get All Semeter
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Semester>> GetAllAsync()
        {
            try
            {
                return await _semesterRepo.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, SemesterMessages.GetAll);
                throw;
            }
        }
    }
}
