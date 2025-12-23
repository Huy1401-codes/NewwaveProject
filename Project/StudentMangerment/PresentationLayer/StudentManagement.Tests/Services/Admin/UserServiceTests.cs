using BusinessLogicLayer.DTOs.Admin;
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
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace StudentManagement.Tests.Services.Admin
{
    public class UserServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IRoleRepository> _roleRepoMock;
        private readonly Mock<ILogger<UserService>> _loggerMock;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepoMock = new Mock<IUserRepository>();
            _roleRepoMock = new Mock<IRoleRepository>();
            _loggerMock = new Mock<ILogger<UserService>>();

            _unitOfWorkMock.Setup(u => u.Users).Returns(_userRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Roles).Returns(_roleRepoMock.Object);

            _service = new UserService(
                _unitOfWorkMock.Object,
                _loggerMock.Object
            );
        }

        #region AddAsync

        [Fact]
        public async Task AddAsync_MissingRequiredFields_ReturnValidationErrors()
        {
            var dto = new UserCreateDto();

            var result = await _service.AddAsync(dto);

            result.Success.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task AddAsync_EmailAlreadyExists_ReturnEmailExists()
        {
            var dto = ValidCreateDto();

            _userRepoMock
                .SetupSequence(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new User()) // email exists
                .ReturnsAsync(null);

            var result = await _service.AddAsync(dto);

            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(UserMessages.EmailExists);
        }

        [Fact]
        public async Task AddAsync_PhoneAlreadyExists_ReturnPhoneExists()
        {
            var dto = ValidCreateDto();

            _userRepoMock
                .SetupSequence(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync((User)null)
                .ReturnsAsync(new User()); 

            var result = await _service.AddAsync(dto);

            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(UserMessages.PhoneExists);
        }

        [Fact]
        public async Task AddAsync_ValidData_ReturnSuccess()
        {
            var dto = ValidCreateDto();

            _userRepoMock
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync((User)null);

            _userRepoMock
                .Setup(r => r.AddAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock
               .Setup(u => u.SaveAsync())
                  .Returns(Task.FromResult(1));


            var result = await _service.AddAsync(dto);

            result.Success.Should().BeTrue();
            result.Errors.Should().BeNull();
            _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.AtLeastOnce());
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_UserNotFound_ReturnError()
        {
            var dto = ValidUpdateDto();

            _userRepoMock.Setup(r => r.GetByIdAsync(dto.UserId)).ReturnsAsync((User)null);

            var result = await _service.UpdateAsync(dto);

            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(UserMessages.UserNotFound);
        }

        [Fact]
        public async Task UpdateAsync_InvalidData_ReturnValidationErrors()
        {
            var dto = new UserUpdateDto { UserId = 1 };

            _userRepoMock.Setup(r => r.GetByIdAsync(dto.UserId)).ReturnsAsync(new User { Id = 1, UserRoles = new List<UserRole>() });

            var result = await _service.UpdateAsync(dto);

            result.Success.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task UpdateAsync_ValidData_ReturnSuccess()
        {
            var dto = ValidUpdateDto();

            var user = new User
            {
                Id = dto.UserId,
                Username = "oldname",
                UserRoles = new List<UserRole> { new UserRole { RoleId = 1 } }
            };

            _userRepoMock.Setup(r => r.GetByIdAsync(dto.UserId)).ReturnsAsync(user);
            _userRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                         .ReturnsAsync((User)null);
            _unitOfWorkMock
                .Setup(u => u.SaveAsync())
                .Returns(Task.FromResult(1));

            var result = await _service.UpdateAsync(dto);

            result.Success.Should().BeTrue();
            result.Errors.Should().BeNull();
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }

        #endregion

        #region ResetPasswordAsync

        [Fact]
        public async Task ResetPasswordAsync_UserNotFound_ReturnFalse()
        {
            _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((User)null);

            var result = await _service.ResetPasswordAsync(1, "NewPass@123");

            result.Should().BeFalse();
        }

        [Fact]
        public async Task ResetPasswordAsync_ValidUser_ReturnTrue()
        {
            var user = new User { Id = 1 };

            _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
            _userRepoMock.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask);
            _unitOfWorkMock
                .Setup(u => u.SaveAsync())
                .Returns(Task.FromResult(1));

            var result = await _service.ResetPasswordAsync(1, "NewPass@123");

            result.Should().BeTrue();
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }

        #endregion

        #region DeleteAsync

        [Fact]
        public async Task DeleteAsync_ValidId_NotThrow()
        {
            _userRepoMock.Setup(r => r.SoftDeleteAsync(1)).Returns(Task.CompletedTask);
            _unitOfWorkMock
                .Setup(u => u.SaveAsync())
                .Returns(Task.FromResult(1));

            var act = async () => await _service.DeleteAsync(1);

            await act.Should().NotThrowAsync();
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }

        #endregion

        #region Helpers

        private UserCreateDto ValidCreateDto() => new UserCreateDto
        {
            Username = "admin01",
            Password = "Admin@123",
            FullName = "Admin User",
            Email = "admin@test.com",
            Phone = "0123456789",
            IsStatus = true,
            RoleId = 1
        };

        private UserUpdateDto ValidUpdateDto() => new UserUpdateDto
        {
            UserId = 1,
            Username = "admin02",
            FullName = "Admin Updated",
            Email = "admin@test.com",
            Phone = "0123456789",
            IsStatus = true,
            RoleId = 2
        };

        #endregion
    }
}
