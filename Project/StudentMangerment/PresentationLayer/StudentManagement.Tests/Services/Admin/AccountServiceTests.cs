using BusinessLogicLayer.Messages.Admin;
using BusinessLogicLayer.Services.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using DataAccessLayer.UnitOfWork;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace StudentManagement.Tests.Services.Admin
{
    public class AccountServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IAccountRepository> _accountRepoMock;
        private readonly Mock<ILogger<AccountService>> _loggerMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly AccountService _service;

        public AccountServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _accountRepoMock = new Mock<IAccountRepository>();
            _loggerMock = new Mock<ILogger<AccountService>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _configurationMock = new Mock<IConfiguration>();

            _unitOfWorkMock
                .Setup(u => u.Accounts)
                .Returns(_accountRepoMock.Object);

            _service = new AccountService(
                _unitOfWorkMock.Object,
                _loggerMock.Object,
                _httpContextAccessorMock.Object,
                _configurationMock.Object
            );
        }

        [Fact]
        public async Task LoginAsync_EmailNotExist_ReturnEmailFail()
        {
            _accountRepoMock
                .Setup(r => r.GetByUsernameAsync("test@gmail.com"))
                .ReturnsAsync((User)null);

            var result = await _service.LoginAsync("test@gmail.com", "123");

            result.User.Should().BeNull();
            result.ErrorMessage.Should().Be(LoginMessages.EmailFail);
        }

        [Fact]
        public async Task LoginAsync_AccountInactive_ReturnInActiveMessage()
        {
            var user = new User
            {
                Email = "inactive@test.com",
                IsStatus = false, // inactive
                PasswordHash = "123456"
            };

            _accountRepoMock
                .Setup(r => r.GetByUsernameAsync(user.Email))
                .ReturnsAsync(user);

            var result = await _service.LoginAsync(user.Email, "123456");

            result.User.Should().BeNull();
            result.ErrorMessage.Should().Be(LoginMessages.InActive);
        }

        [Fact]
        public async Task LoginAsync_BCryptPasswordCorrect_ReturnUser()
        {
            var password = "123456";
            var hash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Email = "active@test.com",
                IsStatus = true,
                PasswordHash = hash
            };

            _accountRepoMock
                .Setup(r => r.GetByUsernameAsync(user.Email))
                .ReturnsAsync(user);

            var result = await _service.LoginAsync(user.Email, password);

            result.User.Should().NotBeNull();
            result.ErrorMessage.Should().BeNull();
        }

        [Fact]
        public async Task LoginAsync_BCryptPasswordWrong_ReturnPasswordFail()
        {
            var hash = BCrypt.Net.BCrypt.HashPassword("correctpass");

            var user = new User
            {
                Email = "active@test.com",
                IsStatus = true,
                PasswordHash = hash
            };

            _accountRepoMock
                .Setup(r => r.GetByUsernameAsync(user.Email))
                .ReturnsAsync(user);

            var result = await _service.LoginAsync(user.Email, "wrongpass");

            result.User.Should().BeNull();
            result.ErrorMessage.Should().Be(LoginMessages.PasswordFail);
        }

        [Fact]
        public async Task LoginAsync_PlainPasswordCorrect_ReturnUser()
        {
            var user = new User
            {
                Email = "active@test.com",
                IsStatus = true,
                PasswordHash = "123456"
            };

            _accountRepoMock
                .Setup(r => r.GetByUsernameAsync(user.Email))
                .ReturnsAsync(user);

            var result = await _service.LoginAsync(user.Email, "123456");

            result.User.Should().NotBeNull();
            result.ErrorMessage.Should().BeNull();
        }

        [Fact]
        public async Task LoginAsync_PlainPasswordWrong_ReturnPasswordFail()
        {
            var user = new User
            {
                Email = "active@test.com",
                IsStatus = true,
                PasswordHash = "123456"
            };

            _accountRepoMock
                .Setup(r => r.GetByUsernameAsync(user.Email))
                .ReturnsAsync(user);

            var result = await _service.LoginAsync(user.Email, "wrongpass");

            result.User.Should().BeNull();
            result.ErrorMessage.Should().Be(LoginMessages.PasswordFail);
        }
    }

}
