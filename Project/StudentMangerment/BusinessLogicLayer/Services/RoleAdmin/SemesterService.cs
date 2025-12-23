using BusinessLogicLayer.DTOs.Admin;
using BusinessLogicLayer.Messages;
using BusinessLogicLayer.Messages.Admin;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.Services.RoleAdmin
{
    public class SemesterService : ISemesterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SemesterService> _logger;

        public SemesterService(IUnitOfWork unitOfWork, ILogger<SemesterService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #region Get Paged Semesters
        public async Task<(IEnumerable<SemesterDto> Semesters, int Total)> GetPagedSemestersAsync(
            string search, int pageIndex, int pageSize)
        {
            try
            {
                var query = _unitOfWork.Semesters.Query();

                if (!string.IsNullOrWhiteSpace(search))
                    query = query.Where(s => s.Name.Contains(search));

                int total = await query.CountAsync();

                var data = await query
                    .OrderBy(s => s.StartDate)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .Select(s => new SemesterDto
                    {
                        SemesterId = s.Id,
                        Name = s.Name,
                        StartDate = s.StartDate,
                        EndDate = s.EndDate
                    })
                    .ToListAsync();

                _logger.LogInformation(SemesterMessages.GetPagedSuccess);
                return (data, total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, SemesterMessages.GetPagedError);
                throw;
            }
        }
        #endregion

        #region Get By Id
        public async Task<SemesterDto?> GetByIdAsync(int id)
        {
            try
            {
                var s = await _unitOfWork.Semesters.GetByIdAsync(id);
                if (s == null)
                {
                    _logger.LogWarning(SemesterMessages.NotFound, id);
                    return null;
                }

                return new SemesterDto
                {
                    SemesterId = s.Id,
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
        #endregion

        #region Add Semester
        public async Task AddAsync(SemesterCreateDto dto)
        {
            try
            {
                if (dto.StartDate >= dto.EndDate)
                {
                    _logger.LogWarning(SemesterMessages.InvalidDate);
                    throw new ArgumentException(SemesterMessages.InvalidDate);
                }

                bool isExist = await _unitOfWork.Semesters.AnyAsync(s =>
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

                await _unitOfWork.Semesters.AddAsync(semester);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation(SemesterMessages.CreateSuccess);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, SemesterMessages.AddError, dto.Name);
                throw;
            }
        }
        #endregion

        #region Update Semester
        public async Task UpdateAsync(SemesterDto dto)
        {
            try
            {
                if (dto.StartDate >= dto.EndDate)
                {
                    _logger.LogWarning(SemesterMessages.InvalidDate);
                    throw new ArgumentException(SemesterMessages.InvalidDate);
                }

                var semester = await _unitOfWork.Semesters.GetByIdAsync(dto.SemesterId);
                if (semester == null)
                {
                    _logger.LogWarning(SemesterMessages.NotFound, dto.SemesterId);
                    return;
                }

                bool isExist = await _unitOfWork.Semesters.AnyAsync(s =>
                    s.Id != dto.SemesterId &&
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

                await _unitOfWork.Semesters.UpdateAsync(semester);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation(SemesterMessages.UpdateSuccess);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, SemesterMessages.UpdateError, dto.SemesterId);
                throw;
            }
        }
        #endregion

        #region Get All
        public async Task<IEnumerable<Semester>> GetAllAsync()
        {
            try
            {
                return await _unitOfWork.Semesters.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, SemesterMessages.GetAll);
                throw;
            }
        }
        #endregion
    }
}
