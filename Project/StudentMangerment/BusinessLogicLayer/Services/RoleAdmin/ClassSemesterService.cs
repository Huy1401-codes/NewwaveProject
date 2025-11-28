using BusinessLogicLayer.DTOs.Admin.ManagerClass;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;

namespace BusinessLogicLayer.Services.RoleAdmin
{
    public class ClassSemesterService : IClassSemesterService
    {
        private readonly IClassSemesterRepository _classRepo;

        public ClassSemesterService(IClassSemesterRepository classRepo)
        {
            _classRepo = classRepo;
        }

        public async Task<bool> CreateAsync(ClassCreateDto dto)
        {
            var entity = new Class
            {
                ClassName = dto.ClassName,
                IsStatus = dto.IsStatus,
                SubjectId = dto.SubjectId,
                SemesterId = dto.SemesterId,
                TeacherId = dto.TeacherId,
            };

            foreach (var studentId in dto.StudentIds)
            {
                entity.ClassStudents.Add(new ClassStudent
                {
                    StudentId = studentId
                });
            }

            await _classRepo.AddAsync(entity);
            return true;
        }

        public async Task<bool> UpdateAsync(ClassUpdateDto dto)
        {
            var entity = await _classRepo.GetByIdAsync(dto.ClassId);
            if (entity == null) return false;

            entity.ClassName = dto.ClassName;
            entity.IsStatus = dto.IsStatus;
            entity.SubjectId = dto.SubjectId;
            entity.SemesterId = dto.SemesterId;
            entity.TeacherId = dto.TeacherId;

            entity.ClassStudents.Clear();

            foreach (var studentId in dto.StudentIds)
            {
                entity.ClassStudents.Add(new ClassStudent { StudentId = studentId });
            }

            await _classRepo.UpdateAsync(entity);
            return true;
        }

        public Task<bool> DeleteAsync(int id)
            => _classRepo.DeleteAsync(id).ContinueWith(_ => true);

        public async Task<ClassDetailDto> GetByIdAsync(int id)
        {
            var entity = await _classRepo.GetByIdAsync(id);
            if (entity == null) return null;

            return new ClassDetailDto
            {
                ClassId = entity.ClassId,
                ClassName = entity.ClassName,
                IsStatus = entity.IsStatus,
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

        public async Task<List<ClassDetailDto>> GetAllAsync()
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
    }


}
