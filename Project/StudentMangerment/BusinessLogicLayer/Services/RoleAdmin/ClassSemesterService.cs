using BusinessLogicLayer.DTOs.Admin.ManagerClass;
using BusinessLogicLayer.Enums.Admin;
using BusinessLogicLayer.Messages.Admin;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.Services.RoleAdmin
{
    public class ClassSemesterService : IClassSemesterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ClassSemesterService> _logger;

        public ClassSemesterService(IUnitOfWork unitOfWork, ILogger<ClassSemesterService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> CreateAsync(ClassCreateDto dto)
        {
            try
            {
                var entity = new Class
                {
                    Name = dto.ClassName,
                    IsStatus = dto.IsStatus == ClassStatus.Active,
                    SubjectId = dto.SubjectId,
                    SemesterId = dto.SemesterId,
                    TeacherId = dto.TeacherId,
                    ClassStudents = dto.StudentIds?.Select(sid => new ClassStudent
                    {
                        StudentId = sid,
                        EnrollDate = DateTime.UtcNow
                    }).ToList() ?? new List<ClassStudent>(),
                    ClassSemesters = new List<ClassSemester>
                    {
                        new ClassSemester
                        {
                            SemesterId = dto.SemesterId,
                            IsStatus = true
                        }
                    }
                };

                await _unitOfWork.ClassSemesters.AddAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation(ClassMessages.CreateSuccess);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ClassMessages.CreateError, dto.ClassName);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(ClassUpdateDto dto)
        {
            try
            {
                var entity = await _unitOfWork.ClassSemesters.GetByIdAsync(dto.ClassId);
                if (entity == null) return false;

                entity.Name = dto.ClassName;
                entity.IsStatus = dto.IsStatus == ClassStatus.Active;
                entity.SubjectId = dto.SubjectId;
                entity.SemesterId = dto.SemesterId;
                entity.TeacherId = dto.TeacherId;

                var existingStudentIds = entity.ClassStudents
                    .Select(cs => cs.StudentId)
                    .ToHashSet();

                if (dto.StudentIds != null)
                {
                    foreach (var studentId in dto.StudentIds.Distinct())
                    {
                        if (!existingStudentIds.Contains(studentId))
                        {
                            entity.ClassStudents.Add(new ClassStudent
                            {
                                ClassId = entity.Id,
                                StudentId = studentId,
                                EnrollDate = DateTime.UtcNow
                            });
                        }
                    }
                }

                await _unitOfWork.ClassSemesters.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation(ClassMessages.UpdateSuccess);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ClassMessages.UpdateError, dto.ClassId);
                throw;
            }
        }


        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _unitOfWork.ClassSemesters.DeleteAsync(id);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation(ClassMessages.DeleteSuccess);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ClassMessages.DeleteError, id);
                throw;
            }
        }

        public async Task<ClassDetailDto> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.ClassSemesters.GetByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning(ClassMessages.ClassNotFound, id);
                    return null;
                }

                _logger.LogInformation(ClassMessages.GetByIdSuccess);
                return MapToDetailDto(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ClassMessages.GetByIdError, id);
                throw;
            }
        }

        public async Task<List<ClassDetailDto>> GetAllAsync()
        {
            try
            {
                var list = await _unitOfWork.ClassSemesters.GetAllAsync();
                _logger.LogInformation(ClassMessages.GetAllSuccess);

                return list.Select(MapToDetailDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ClassMessages.GetAllError);
                throw;
            }
        }

        private ClassDetailDto MapToDetailDto(Class entity)
        {
            return new ClassDetailDto
            {
                ClassId = entity.Id,
                ClassName = entity.Name,
                IsStatus = entity.IsStatus,
                SubjectId = entity.SubjectId,
                SubjectName = entity.Subject?.Name ?? "[No Subject]",
                SemesterId = entity.SemesterId,
                SemesterName = entity.Semester?.Name ?? "[No Semester]",
                TeacherId = entity.TeacherId,
                TeacherName = entity.Teacher?.User?.FullName ?? "[No Teacher]",
                Students = entity.ClassStudents?.Select(cs => new StudentInClassDto
                {
                    StudentId = cs.StudentId,
                    FullName = cs.Student?.User?.FullName ?? "[No Name]"
                }).ToList() ?? new List<StudentInClassDto>()
            };
        }
    }
}
