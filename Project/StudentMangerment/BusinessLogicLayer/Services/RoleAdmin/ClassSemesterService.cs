using BusinessLogicLayer.DTOs.Admin.ManagerClass;
using BusinessLogicLayer.Messages.Admin;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.Services.RoleAdmin
{
    public class ClassSemesterService : IClassSemesterService
    {
        private readonly IClassSemesterRepository _classRepo;
        private readonly ILogger<ClassSemesterService> _logger;

        public ClassSemesterService(IClassSemesterRepository classRepo, ILogger<ClassSemesterService> logger)
        {
            _classRepo = classRepo;
            _logger = logger;
        }

        public async Task<bool> CreateAsync(ClassCreateDto dto)
        {
            try
            {
                var entity = new Class
                {
                    ClassName = dto.ClassName,
                    IsStatus = dto.IsStatus,
                    SubjectId = dto.SubjectId,
                    SemesterId = dto.SemesterId,
                    TeacherId = dto.TeacherId,
                    ClassStudents = new List<ClassStudent>()
                };

                if (dto.StudentIds != null)
                {
                    foreach (var studentId in dto.StudentIds)
                    {
                        entity.ClassStudents.Add(new ClassStudent
                        {
                            StudentId = studentId,
                            EnrollDate = DateTime.Now
                        });
                    }
                }
                entity.ClassSemesters.Add(new ClassSemester
                {
                    SemesterId = dto.SemesterId,
                    IsStatus = true
                });

                await _classRepo.AddAsync(entity);
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
                var entity = await _classRepo.GetByIdAsync(dto.ClassId);
                if (entity == null) return false;

                entity.ClassName = dto.ClassName;
                entity.IsStatus = dto.IsStatus;
                entity.SubjectId = dto.SubjectId;
                entity.SemesterId = dto.SemesterId;
                entity.TeacherId = dto.TeacherId;

                entity.ClassStudents.Clear();
                entity.ClassSemesters.Add(new ClassSemester
                {
                    SemesterId = dto.SemesterId,
                    IsStatus = true
                });

                if (dto.StudentIds != null)
                {
                    foreach (var studentId in dto.StudentIds)
                    {
                        entity.ClassStudents.Add(new ClassStudent
                        {
                            StudentId = studentId,
                            EnrollDate = DateTime.UtcNow
                        });
                    }
                }

                await _classRepo.UpdateAsync(entity);
                return true;
            }
            catch (Exception ex)
            {
                {
                    _logger.LogError(ex, ClassMessages.UpdateError, dto.ClassId);
                    throw;
                }
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _classRepo.DeleteAsync(id);
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
                var entity = await _classRepo.GetByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning(ClassMessages.ClassNotFound, id);
                    return null;
                }

                return new ClassDetailDto
                {
                    ClassId = entity.ClassId,
                    ClassName = entity.ClassName,
                    IsStatus = entity.IsStatus,
                    SubjectId = entity.SubjectId,
                    SemesterId = entity.SemesterId,
                    TeacherId = entity.TeacherId,
                    SubjectName = entity.Subject?.Name,
                    SemesterName = entity.Semester?.Name,
                    TeacherName = entity.Teacher?.User.FullName,
                    Students = entity.ClassStudents.Select(x => new StudentInClassDto
                    {
                        StudentId = x.StudentId,
                        FullName = x.Student.User.FullName
                    }).ToList()
                };
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
                var list = await _classRepo.GetAllAsync();

                return list.Select(entity => new ClassDetailDto
                {
                    ClassId = entity.ClassId,
                    ClassName = entity.ClassName,
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
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ClassMessages.GetAllError);
                throw;
            }
        }
    }
}
