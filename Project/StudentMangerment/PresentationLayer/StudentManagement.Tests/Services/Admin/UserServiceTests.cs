using BusinessLogicLayer.DTOs.Admin;
using BusinessLogicLayer.Messages.Admin;
using BusinessLogicLayer.Services.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace StudentManagement.Tests.Services.Admin
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IRoleRepository> _roleRepoMock;
        private readonly Mock<ILogger<UserService>> _loggerMock;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _roleRepoMock = new Mock<IRoleRepository>();
            _loggerMock = new Mock<ILogger<UserService>>();

            _service = new UserService(
                _userRepoMock.Object,
                _roleRepoMock.Object,
                _loggerMock.Object
            );
        }

        #region AddAsync

        [Fact]
        public async Task AddAsync_MissingRequiredFields_ReturnValidationErrors()
        {
            // Trường hợp: Thiếu dữ liệu bắt buộc

            // Arrange
            var dto = new UserCreateDto();

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task AddAsync_EmailAlreadyExists_ReturnEmailExists()
        {
            // Trường hợp: Email bị trùng

            // Arrange
            var dto = ValidCreateDto();

            _userRepoMock
                      .SetupSequence(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                      .ReturnsAsync((User?)new User()) 
                      .ReturnsAsync(null);


            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(UserMessages.EmailExists);
        }

        [Fact]
        public async Task AddAsync_PhoneAlreadyExists_ReturnPhoneExists()
        {
            // Trường hợp: Phone bị trùng

            // Arrange
            var dto = ValidCreateDto();

            _userRepoMock
                     .SetupSequence(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                     .ReturnsAsync((User?)null)
                     .ReturnsAsync(new User());


            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(UserMessages.PhoneExists);
        }

        [Fact]
        public async Task AddAsync_ValidData_ReturnSuccess()
        {
            // Trường hợp: Tạo user hợp lệ

            // Arrange
            var dto = ValidCreateDto();

            _userRepoMock
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync((User)null);

            _userRepoMock
                .Setup(r => r.AddAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            _userRepoMock
                .Setup(r => r.SaveAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            result.Success.Should().BeTrue();
            result.Errors.Should().BeNull();
        }

        #endregion


        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_UserNotFound_ReturnError()
        {
            // Trường hợp: User không tồn tại

            // Arrange
            var dto = ValidUpdateDto();

            _userRepoMock
                .Setup(r => r.GetByIdAsync(dto.UserId))
                .ReturnsAsync((User)null);

            // Act
            var result = await _service.UpdateAsync(dto);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(UserMessages.UserNotFound);
        }

        [Fact]
        public async Task UpdateAsync_InvalidData_ReturnValidationErrors()
        {
            // Trường hợp: Validate update fail

            // Arrange
            var dto = new UserUpdateDto { UserId = 1 };

            _userRepoMock
                .Setup(r => r.GetByIdAsync(dto.UserId))
                .ReturnsAsync(new User { Id = 1, UserRoles = new List<UserRole>() });

            // Act
            var result = await _service.UpdateAsync(dto);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task UpdateAsync_ValidData_ReturnSuccess()
        {
            // Trường hợp: Update user hợp lệ

            // Arrange
            var dto = ValidUpdateDto();

            var user = new User
            {
                Id = dto.UserId,
                Username = "oldname",
                UserRoles = new List<UserRole>
                {
                    new UserRole { RoleId = 1 }
                }
            };

            _userRepoMock
                .Setup(r => r.GetByIdAsync(dto.UserId))
                .ReturnsAsync(user);

            _userRepoMock
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync((User)null);

            _userRepoMock
                .Setup(r => r.SaveAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateAsync(dto);

            // Assert
            result.Success.Should().BeTrue();
            result.Errors.Should().BeNull();
        }

        #endregion


        #region ResetPasswordAsync

        [Fact]
        public async Task ResetPasswordAsync_UserNotFound_ReturnFalse()
        {
            // Trường hợp: Reset password cho user không tồn tại

            _userRepoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((User)null);

            var result = await _service.ResetPasswordAsync(1, "NewPass@123");

            result.Should().BeFalse();
        }

        [Fact]
        public async Task ResetPasswordAsync_ValidUser_ReturnTrue()
        {
            // Trường hợp: Reset password hợp lệ

            var user = new User { Id = 1 };

            _userRepoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(user);

            _userRepoMock
                .Setup(r => r.UpdateAsync(user))
                .Returns(Task.CompletedTask);

            _userRepoMock
                .Setup(r => r.SaveAsync())
                .Returns(Task.CompletedTask);

            var result = await _service.ResetPasswordAsync(1, "NewPass@123");

            result.Should().BeTrue();
        }

        #endregion


        #region DeleteAsync

        [Fact]
        public async Task DeleteAsync_ValidId_NotThrow()
        {
            // Trường hợp: Xóa user không phát sinh lỗi

            _userRepoMock
                .Setup(r => r.SoftDeleteAsync(1))
                .Returns(Task.CompletedTask);

            _userRepoMock
                .Setup(r => r.SaveAsync())
                .Returns(Task.CompletedTask);

            var act = async () => await _service.DeleteAsync(1);

            await act.Should().NotThrowAsync();
        }

        #endregion


        #region Helpers

        private UserCreateDto ValidCreateDto()
        {
            return new UserCreateDto
            {
                Username = "admin01",
                Password = "Admin@123",
                FullName = "Admin User",
                Email = "admin@test.com",
                Phone = "0123456789",
                IsStatus = true,
                RoleId = 1
            };
        }

        private UserUpdateDto ValidUpdateDto()
        {
            return new UserUpdateDto
            {
                UserId = 1,
                Username = "admin02",
                FullName = "Admin Updated",
                Email = "admin@test.com",
                Phone = "0123456789",
                IsStatus = true,
                RoleId = 2
            };
        }

        #endregion
    }
}
