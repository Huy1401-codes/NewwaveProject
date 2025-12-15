using BusinessLogicLayer.DTOs.Admin.ManagerClass;
using BusinessLogicLayer.Enums.Admin;
using BusinessLogicLayer.Services.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace StudentManagement.Tests.Services.Admin
{
    public class ClassSemesterServiceTests
    {
        private readonly Mock<IClassSemesterRepository> _repoMock;
        private readonly Mock<ILogger<ClassSemesterService>> _loggerMock;
        private readonly ClassSemesterService _service;

        public ClassSemesterServiceTests()
        {
            _repoMock = new Mock<IClassSemesterRepository>();
            _loggerMock = new Mock<ILogger<ClassSemesterService>>();
            _service = new ClassSemesterService(_repoMock.Object, _loggerMock.Object);
        }

        #region Create
        [Fact]
        public async Task CreateAsync_ValidDto_ReturnTrue()
        {          

            // Arrange
            var dto = new ClassCreateDto
            {
                ClassName = "10A1",
                IsStatus = ClassStatus.Active,
                SubjectId = 1,
                SemesterId = 1,
                TeacherId = 1,
                StudentIds = new List<int> { 1, 2 }
            };

            _repoMock
                .Setup(r => r.AddAsync(It.IsAny<Class>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            result.Should().BeTrue();
        }
        #endregion

        #region Update
        [Fact]
        public async Task UpdateAsync_ClassNotFound_ReturnFalse()
        {
            // Arrange
            var dto = new ClassUpdateDto
            {
                ClassId = 1
            };

            _repoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Class)null);

            // Act
            var result = await _service.UpdateAsync(dto);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAsync_ValidClass_ReturnTrue()
        {

            // Arrange
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

            _repoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(entity);

            _repoMock
                .Setup(r => r.UpdateAsync(entity))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateAsync(dto);

            // Assert
            result.Should().BeTrue();
        }
        #endregion

        #region Delete
        [Fact]
        public async Task DeleteAsync_ValidId_ReturnTrue()
        {
            // Arrange
            _repoMock
                .Setup(r => r.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            result.Should().BeTrue();
        }
        #endregion
    }
}
