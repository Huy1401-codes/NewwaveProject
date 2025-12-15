using BusinessLogicLayer.DTOs.ManagerTeacher;
using BusinessLogicLayer.Messages.Admin;
using BusinessLogicLayer.Services.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace StudentManagement.Tests.Services.Admin
{
    public class TeacherServiceTests
    {
        private readonly Mock<ITeacherRepository> _teacherRepoMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<ILogger<TeacherService>> _loggerMock;
        private readonly TeacherService _service;

        public TeacherServiceTests()
        {
            _teacherRepoMock = new Mock<ITeacherRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _loggerMock = new Mock<ILogger<TeacherService>>();

            _service = new TeacherService(
                _teacherRepoMock.Object,
                _userRepoMock.Object,
                _loggerMock.Object
            );
        }

        #region CreateAsync

        [Fact]
        public async Task CreateAsync_UserNotInTeacherRole_ReturnInvalidUser()
        {
            // Trường hợp: User không thuộc role Teacher

            // Arrange
            var dto = new CreateTeacherDto
            {
                UserId = 1,
                TeacherCode = "TC01",
                Degree = "Master"
            };

            _teacherRepoMock
                .Setup(r => r.GetUsersByRoleAsync("Teacher"))
                .ReturnsAsync(new List<User>());

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().Be(TeacherMessages.InvalidUser);
        }

        [Fact]
        public async Task CreateAsync_DuplicateUser_ReturnDuplicateUser()
        {
            // Trường hợp: User đã được gán làm Teacher trước đó

            // Arrange
            var dto = new CreateTeacherDto
            {
                UserId = 1,
                TeacherCode = "TC01",
                Degree = "Master"
            };

            _teacherRepoMock
                .Setup(r => r.GetUsersByRoleAsync("Teacher"))
                .ReturnsAsync(new List<User>
                {
                    new User { Id = 1 }
                });

            _teacherRepoMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Teacher>
                {
                    new Teacher { UserId = 1 }
                });

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().Be(TeacherMessages.DuplicateUser);
        }

        [Fact]
        public async Task CreateAsync_DuplicateTeacherCode_ReturnDuplicateCode()
        {
            // Trường hợp: TeacherCode bị trùng

            // Arrange
            var dto = new CreateTeacherDto
            {
                UserId = 1,
                TeacherCode = "TC01",
                Degree = "Master"
            };

            _teacherRepoMock
                .Setup(r => r.GetUsersByRoleAsync("Teacher"))
                .ReturnsAsync(new List<User>
                {
                    new User { Id = 1 }
                });

            _teacherRepoMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Teacher>
                {
                    new Teacher { UserId = 2, TeacherCode = "TC01" }
                });

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().Be(TeacherMessages.DuplicateCode);
        }

        [Fact]
        public async Task CreateAsync_ValidData_ReturnSuccess()
        {
            // Trường hợp: Tạo Teacher hợp lệ

            // Arrange
            var dto = new CreateTeacherDto
            {
                UserId = 1,
                TeacherCode = "TC01",
                Degree = "Master"
            };

            _teacherRepoMock
                .Setup(r => r.GetUsersByRoleAsync("Teacher"))
                .ReturnsAsync(new List<User>
                {
                    new User { Id = 1 }
                });

            _teacherRepoMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Teacher>());

            _teacherRepoMock
                .Setup(r => r.AddAsync(It.IsAny<Teacher>()))
                .Returns(Task.CompletedTask);

            _teacherRepoMock
                .Setup(r => r.SaveAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            result.Success.Should().BeTrue();
            result.ErrorMessage.Should().BeEmpty();
        }

        #endregion


        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_TeacherNotFound_ReturnFalse()
        {
            // Trường hợp: Không tìm thấy Teacher để cập nhật

            // Arrange
            var teacher = new Teacher
            {
                Id = 1,
                Degree = "PhD"
            };

            _teacherRepoMock
                .Setup(r => r.GetByIdAsync(teacher.Id))
                .ReturnsAsync((Teacher)null);

            // Act
            var result = await _service.UpdateAsync(teacher);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAsync_ValidTeacher_ReturnTrue()
        {
            // Trường hợp: Cập nhật Teacher hợp lệ

            // Arrange
            var teacher = new Teacher
            {
                Id = 1,
                Degree = "PhD"
            };

            var existing = new Teacher
            {
                Id = 1,
                Degree = "Master"
            };

            _teacherRepoMock
                .Setup(r => r.GetByIdAsync(teacher.Id))
                .ReturnsAsync(existing);

            _teacherRepoMock
                .Setup(r => r.UpdateAsync(It.IsAny<Teacher>()))
                .Returns(Task.CompletedTask);

            _teacherRepoMock
                .Setup(r => r.SaveAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateAsync(teacher);

            // Assert
            result.Should().BeTrue();
        }

        #endregion


        #region SoftDeleteAsync

        [Fact]
        public async Task SoftDeleteAsync_TeacherNotFound_ReturnFalse()
        {
            // Trường hợp: Xóa Teacher nhưng không tồn tại

            // Arrange
            _teacherRepoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Teacher)null);

            // Act
            var result = await _service.SoftDeleteAsync(1);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task SoftDeleteAsync_ValidTeacher_ReturnTrue()
        {
            // Trường hợp: Xóa Teacher hợp lệ

            // Arrange
            _teacherRepoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Teacher { Id = 1 });

            _teacherRepoMock
                .Setup(r => r.SoftDeleteAsync(1))
                .Returns(Task.CompletedTask);

            _teacherRepoMock
                .Setup(r => r.SaveAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.SoftDeleteAsync(1);

            // Assert
            result.Should().BeTrue();
        }

        #endregion
    }
}
