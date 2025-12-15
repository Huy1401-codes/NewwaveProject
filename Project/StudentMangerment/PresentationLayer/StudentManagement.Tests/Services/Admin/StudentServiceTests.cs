using BusinessLogicLayer.DTOs.ManagerStudent;
using BusinessLogicLayer.Messages.Admin;
using BusinessLogicLayer.Services.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace StudentManagement.Tests.Services.Admin
{
    public class StudentServiceTests
    {
        private readonly Mock<IStudentRepository> _studentRepoMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<ILogger<StudentService>> _loggerMock;
        private readonly StudentService _service;

        public StudentServiceTests()
        {
            _studentRepoMock = new Mock<IStudentRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _loggerMock = new Mock<ILogger<StudentService>>();

            _service = new StudentService(
                _studentRepoMock.Object,
                _userRepoMock.Object,
                _loggerMock.Object);
        }

        #region Create
        [Fact]
        public async Task CreateAsync_UserNotExist_ReturnInvalidUser()
        {
            // Arrange
            var dto = new CreateStudentDto
            {
                UserId = 1,
                StudentCode = "SV001"
            };

            _studentRepoMock
                .Setup(r => r.GetUsersByRoleAsync("Student"))
                .ReturnsAsync(new List<User>());

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().Be(StudentMessages.InvalidUser);
        }

        [Fact]
        public async Task CreateAsync_UserAlreadyStudent_ReturnError()
        {
            // Arrange
            var dto = new CreateStudentDto
            {
                UserId = 1,
                StudentCode = "SV001"
            };

            _studentRepoMock
                .Setup(r => r.GetUsersByRoleAsync("Student"))
                .ReturnsAsync(new List<User> { new User { Id = 1 } });

            _studentRepoMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Student>
                {
                    new Student { UserId = 1 }
                });

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().Be(StudentMessages.UserAlreadyStudent);
        }

        [Fact]
        public async Task CreateAsync_DuplicateStudentCode_ReturnError()
        {
            // Arrange
            var dto = new CreateStudentDto
            {
                UserId = 2,
                StudentCode = "SV001"
            };

            _studentRepoMock
                .Setup(r => r.GetUsersByRoleAsync("Student"))
                .ReturnsAsync(new List<User> { new User { Id = 2 } });

            _studentRepoMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Student>
                {
                    new Student { UserId = 1, StudentCode = "SV001" }
                });

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().Be(StudentMessages.StudentCodeExists);
        }

        [Fact]
        public async Task CreateAsync_ValidData_ReturnSuccess()
        {
            // Arrange
            var dto = new CreateStudentDto
            {
                UserId = 1,
                StudentCode = "SV002",
                BirthDate = DateTime.Now,
                Gender = "Male"
            };

            _studentRepoMock
                .Setup(r => r.GetUsersByRoleAsync("Student"))
                .ReturnsAsync(new List<User> { new User { Id = 1 } });

            _studentRepoMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Student>());

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            result.Success.Should().BeTrue();
            result.ErrorMessage.Should().BeEmpty();
        }
        #endregion

        #region Update
        [Fact]
        public async Task UpdateAsync_StudentNotFound_ReturnFalse()
        {

            // Arrange
            var student = new Student { Id = 1 };

            _studentRepoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Student)null);

            // Act
            var result = await _service.UpdateAsync(student);

            // Assert
            result.Should().BeFalse();
        }
        #endregion

        #region Delete
        [Fact]
        public async Task SoftDeleteAsync_StudentNotFound_ReturnFalse()
        {

            // Arrange
            _studentRepoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Student)null);

            // Act
            var result = await _service.SoftDeleteAsync(1);

            // Assert
            result.Should().BeFalse();
        }
        #endregion

    }
}
