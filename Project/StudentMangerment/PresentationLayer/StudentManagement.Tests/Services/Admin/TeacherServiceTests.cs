using BusinessLogicLayer.DTOs.ManagerTeacher;
using BusinessLogicLayer.Messages.Admin;
using BusinessLogicLayer.Services.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using DataAccessLayer.UnitOfWork;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace StudentManagement.Tests.Services.Admin
{
    public class TeacherServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ITeacherRepository> _teacherRepoMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<ILogger<TeacherService>> _loggerMock;
        private readonly TeacherService _service;

        public TeacherServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _teacherRepoMock = new Mock<ITeacherRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _loggerMock = new Mock<ILogger<TeacherService>>();

            // UnitOfWork trả về repository mock
            _unitOfWorkMock.Setup(u => u.Teachers).Returns(_teacherRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Users).Returns(_userRepoMock.Object);

            _service = new TeacherService(
                _unitOfWorkMock.Object,
                _loggerMock.Object
            );
        }

        #region CreateAsync

        [Fact]
        public async Task CreateAsync_UserNotInTeacherRole_ReturnInvalidUser()
        {
            var dto = new CreateTeacherDto { UserId = 1, TeacherCode = "TC01", Degree = "Master" };

            _teacherRepoMock.Setup(r => r.GetUsersByRoleAsync("Teacher")).ReturnsAsync(new List<User>());

            var result = await _service.CreateAsync(dto);

            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().Be(TeacherMessages.InvalidUser);
        }

        [Fact]
        public async Task CreateAsync_DuplicateUser_ReturnDuplicateUser()
        {
            var dto = new CreateTeacherDto { UserId = 1, TeacherCode = "TC01", Degree = "Master" };

            _teacherRepoMock.Setup(r => r.GetUsersByRoleAsync("Teacher")).ReturnsAsync(new List<User> { new User { Id = 1 } });
            _teacherRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Teacher> { new Teacher { UserId = 1 } });

            var result = await _service.CreateAsync(dto);

            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().Be(TeacherMessages.DuplicateUser);
        }

        [Fact]
        public async Task CreateAsync_DuplicateTeacherCode_ReturnDuplicateCode()
        {
            var dto = new CreateTeacherDto { UserId = 1, TeacherCode = "TC01", Degree = "Master" };

            _teacherRepoMock.Setup(r => r.GetUsersByRoleAsync("Teacher")).ReturnsAsync(new List<User> { new User { Id = 1 } });
            _teacherRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Teacher> { new Teacher { UserId = 2, TeacherCode = "TC01" } });

            var result = await _service.CreateAsync(dto);

            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().Be(TeacherMessages.DuplicateCode);
        }

        [Fact]
        public async Task CreateAsync_ValidData_ReturnSuccess()
        {
            var dto = new CreateTeacherDto { UserId = 1, TeacherCode = "TC01", Degree = "Master" };

            _teacherRepoMock.Setup(r => r.GetUsersByRoleAsync("Teacher")).ReturnsAsync(new List<User> { new User { Id = 1 } });
            _teacherRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Teacher>());
            _teacherRepoMock.Setup(r => r.AddAsync(It.IsAny<Teacher>())).Returns(Task.CompletedTask);
            _unitOfWorkMock
                .Setup(u => u.SaveAsync())
                .Returns(Task.FromResult(1));

            var result = await _service.CreateAsync(dto);

            result.Success.Should().BeTrue();
            result.ErrorMessage.Should().BeEmpty();
            _teacherRepoMock.Verify(r => r.AddAsync(It.IsAny<Teacher>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_TeacherNotFound_ReturnFalse()
        {
            var teacher = new Teacher { Id = 1, Degree = "PhD" };
            _teacherRepoMock.Setup(r => r.GetByIdAsync(teacher.Id)).ReturnsAsync((Teacher)null);

            var result = await _service.UpdateAsync(teacher);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAsync_ValidTeacher_ReturnTrue()
        {
            var teacher = new Teacher { Id = 1, Degree = "PhD" };
            var existing = new Teacher { Id = 1, Degree = "Master" };

            _teacherRepoMock.Setup(r => r.GetByIdAsync(teacher.Id)).ReturnsAsync(existing);
            _teacherRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Teacher>())).Returns(Task.CompletedTask);
            _unitOfWorkMock
                .Setup(u => u.SaveAsync())
                .Returns(Task.FromResult(1));

            var result = await _service.UpdateAsync(teacher);

            result.Should().BeTrue();
            _teacherRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Teacher>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }

        #endregion

        #region SoftDeleteAsync

        [Fact]
        public async Task SoftDeleteAsync_TeacherNotFound_ReturnFalse()
        {
            _teacherRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Teacher)null);

            var result = await _service.SoftDeleteAsync(1);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task SoftDeleteAsync_ValidTeacher_ReturnTrue()
        {
            _teacherRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Teacher { Id = 1 });
            _teacherRepoMock.Setup(r => r.SoftDeleteAsync(1)).Returns(Task.CompletedTask);
            _unitOfWorkMock
                .Setup(u => u.SaveAsync())
                .Returns(Task.FromResult(1));

            var result = await _service.SoftDeleteAsync(1);

            result.Should().BeTrue();
            _teacherRepoMock.Verify(r => r.SoftDeleteAsync(1), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }

        #endregion
    }
}
