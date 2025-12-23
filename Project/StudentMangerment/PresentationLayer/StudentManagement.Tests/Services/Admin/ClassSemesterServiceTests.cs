using BusinessLogicLayer.DTOs.Admin.ManagerClass;
using BusinessLogicLayer.Enums.Admin;
using BusinessLogicLayer.Services.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using DataAccessLayer.UnitOfWork;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace StudentManagement.Tests.Services.Admin
{
    public class ClassSemesterServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IClassSemesterRepository> _classSemesterRepoMock;
        private readonly Mock<ILogger<ClassSemesterService>> _loggerMock;
        private readonly ClassSemesterService _service;

        public ClassSemesterServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _classSemesterRepoMock = new Mock<IClassSemesterRepository>();
            _loggerMock = new Mock<ILogger<ClassSemesterService>>();

            _unitOfWorkMock
                .Setup(u => u.ClassSemesters)
                .Returns(_classSemesterRepoMock.Object);

            _service = new ClassSemesterService(
                _unitOfWorkMock.Object,
                _loggerMock.Object
            );
        }

        #region Create
        [Fact]
        public async Task CreateAsync_ValidDto_ReturnTrue()
        {
            var dto = new ClassCreateDto
            {
                ClassName = "10A1",
                IsStatus = ClassStatus.Active,
                SubjectId = 1,
                SemesterId = 1,
                TeacherId = 1,
                StudentIds = new List<int> { 1, 2 }
            };

            _classSemesterRepoMock
                .Setup(r => r.AddAsync(It.IsAny<Class>()))
                .Returns(Task.CompletedTask);

            var result = await _service.CreateAsync(dto);

            result.Should().BeTrue();
        }
        #endregion

        #region Update
        [Fact]
        public async Task UpdateAsync_ClassNotFound_ReturnFalse()
        {
            var dto = new ClassUpdateDto
            {
                ClassId = 1
            };

            _classSemesterRepoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Class)null);

            var result = await _service.UpdateAsync(dto);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAsync_ValidClass_ReturnTrue()
        {
            var entity = new Class
            {
                Id = 1,
                ClassStudents = new List<ClassStudent>(),
                ClassSemesters = new List<ClassSemester>()
            };

            var dto = new ClassUpdateDto
            {
                ClassId = 1,
                ClassName = "10A2",
                IsStatus = ClassStatus.Active,
                SubjectId = 1,
                SemesterId = 2,
                TeacherId = 3,
                StudentIds = new List<int> { 1 }
            };

            _classSemesterRepoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(entity);

            _classSemesterRepoMock
                .Setup(r => r.UpdateAsync(entity))
                .Returns(Task.CompletedTask);

            var result = await _service.UpdateAsync(dto);

            result.Should().BeTrue();
        }
        #endregion

        #region Delete
        [Fact]
        public async Task DeleteAsync_ValidId_ReturnTrue()
        {
            _classSemesterRepoMock
                .Setup(r => r.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            var result = await _service.DeleteAsync(1);

            result.Should().BeTrue();
        }
        #endregion
    }
}
