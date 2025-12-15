using BusinessLogicLayer.Messages.Admin;
using BusinessLogicLayer.Services.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace StudentManagement.Tests.Services.Admin
{
    public class AccountServiceTests
    {
        private readonly Mock<IAccountRepository> _accountRepoMock;
        private readonly Mock<ILogger<AccountService>> _loggerMock;
        private readonly AccountService _service;

        public AccountServiceTests()
        {
            _accountRepoMock = new Mock<IAccountRepository>();
            _loggerMock = new Mock<ILogger<AccountService>>();

            _service = new AccountService(
                _accountRepoMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task LoginAsync_EmailNotExist_ReturnEmailFail()
        {
            // Arrange
            _accountRepoMock
                .Setup(r => r.GetByUsernameAsync("test@gmail.com"))
                .ReturnsAsync((User)null);

            // Act
            var result = await _service.LoginAsync("test@gmail.com", "123");

            // Assert
            result.Should().NotBeNull();
            result.User.Should().BeNull();
            result.ErrorMessage.Should().Be(LoginMessages.EmailFail);
        }

        [Fact]
        public async Task LoginAsync_AccountInactive_ReturnInActiveMessage()
        {
            // Arrange
            var user = new User
            {
                Email = "test@gmail.com",
                IsStatus = false
            };

            _accountRepoMock
                .Setup(r => r.GetByUsernameAsync(user.Email))
                .ReturnsAsync(user);

            // Act
            var result = await _service.LoginAsync(user.Email, "123");

            // Assert
            result.ErrorMessage.Should().Be(LoginMessages.InActive);
        }

        [Fact]
        public async Task LoginAsync_BCryptPasswordWrong_ReturnPasswordFail()
        {
            // Arrange
            var hash = BCrypt.Net.BCrypt.HashPassword("correct123");

            var user = new User
            {
                Email = "test@gmail.com",
                IsStatus = true,
                PasswordHash = hash
            };

            _accountRepoMock
                .Setup(r => r.GetByUsernameAsync(user.Email))
                .ReturnsAsync(user);

            // Act
            var result = await _service.LoginAsync(user.Email, "wrong123");

            // Assert
            result.ErrorMessage.Should().Be(LoginMessages.PasswordFail);
        }

        [Fact]
        public async Task LoginAsync_BCryptPasswordCorrect_ReturnUser()
        {
            // Arrange
            var hash = BCrypt.Net.BCrypt.HashPassword("123456");
            var user = new User
            {
                Email = "test@gmail.com",
                IsStatus = true,
                PasswordHash = hash
            };


            _accountRepoMock
                .Setup(r => r.GetByUsernameAsync(user.Email))
                .ReturnsAsync(user);

            // Act
            var result = await _service.LoginAsync(user.Email, "123456");

            // Assert
            result.User.Should().NotBeNull();
            result.ErrorMessage.Should().BeNull();
        }


        [Fact]
        public async Task LoginAsync_PlainPasswordCorrect_ReturnUser()
        {
            // Arrange
            var user = new User
            {
                Email = "test@gmail.com",
                IsStatus = true,
                PasswordHash = "123456"
            };

            _accountRepoMock
                .Setup(r => r.GetByUsernameAsync(user.Email))
                .ReturnsAsync(user);

            // Act
            var result = await _service.LoginAsync(user.Email, "123456");

            // Assert
            result.User.Should().NotBeNull();
        }

        [Fact]
        public async Task LoginAsync_PlainPasswordWrong_ReturnPasswordFail()
        {
            // Arrange
            var user = new User
            {
                Email = "test@gmail.com",
                IsStatus = true,
                PasswordHash = "123456"
            };

            _accountRepoMock
                .Setup(r => r.GetByUsernameAsync(user.Email))
                .ReturnsAsync(user);

            // Act
            var result = await _service.LoginAsync(user.Email, "wrong");

            // Assert
            result.ErrorMessage.Should().Be(LoginMessages.PasswordFail);
        }


    }
}
