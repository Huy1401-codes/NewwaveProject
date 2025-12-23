using BusinessLogicLayer.Services.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using DataAccessLayer.UnitOfWork;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace StudentManagement.Tests.Services.Admin
{
    public class SubjectServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ISubjectRepository> _repoMock;
        private readonly Mock<ILogger<SubjectService>> _loggerMock;
        private readonly SubjectService _service;

        public SubjectServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _repoMock = new Mock<ISubjectRepository>();
            _loggerMock = new Mock<ILogger<SubjectService>>();

            // UnitOfWork trả về repository mock
            _unitOfWorkMock.Setup(u => u.Subjects).Returns(_repoMock.Object);

            _service = new SubjectService(
                _unitOfWorkMock.Object,
                _loggerMock.Object
            );
        }

        #region CreateAsync

        [Fact]
        public async Task CreateAsync_DuplicateName_ReturnFalse()
        {
            // Arrange
            var subject = new Subject
            {
                Name = "Math",
                Credit = 3,
                IsStatus = true
            };

            _repoMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Subject, bool>>>()))
                     .ReturnsAsync(true);

            // Act
            var result = await _service.CreateAsync(subject);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task CreateAsync_ValidSubject_ReturnTrue()
        {
            // Arrange
            var subject = new Subject
            {
                Name = "Physics",
                Credit = 4,
                IsStatus = true
            };

            _repoMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Subject, bool>>>()))
                     .ReturnsAsync(false);

            _repoMock.Setup(r => r.AddAsync(subject)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.FromResult(1));

            // Act
            var result = await _service.CreateAsync(subject);

            // Assert
            result.Should().BeTrue();
            _repoMock.Verify(r => r.AddAsync(subject), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_SubjectNotFound_ReturnFalse()
        {
            // Arrange
            var subject = new Subject
            {
                Id = 1,
                Name = "Math"
            };

            _repoMock.Setup(r => r.GetByIdAsync(subject.Id)).ReturnsAsync((Subject)null);

            // Act
            var result = await _service.UpdateAsync(subject);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAsync_DuplicateName_ReturnFalse()
        {
            // Arrange
            var subject = new Subject
            {
                Id = 2,
                Name = "Math"
            };

            _repoMock.Setup(r => r.GetByIdAsync(subject.Id))
                     .ReturnsAsync(new Subject { Id = 2, Name = "Old Name" });

            _repoMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Subject, bool>>>()))
                     .ReturnsAsync(true);

            // Act
            var result = await _service.UpdateAsync(subject);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAsync_ValidSubject_ReturnTrue()
        {
            // Arrange
            var subject = new Subject
            {
                Id = 1,
                Name = "Math Updated",
                Credit = 3,
                IsStatus = true
            };

            var exist = new Subject
            {
                Id = 1,
                Name = "Math",
                Credit = 2,
                IsStatus = false
            };

            _repoMock.Setup(r => r.GetByIdAsync(subject.Id)).ReturnsAsync(exist);
            _repoMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Subject, bool>>>()))
                     .ReturnsAsync(false);
            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Subject>())).Returns(Task.CompletedTask);
            _unitOfWorkMock
                .Setup(u => u.SaveAsync())
                .Returns(Task.FromResult(1));

            // Act
            var result = await _service.UpdateAsync(subject);

            // Assert
            result.Should().BeTrue();
            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Subject>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }

        #endregion

        #region DeleteAsync

        [Fact]
        public async Task DeleteAsync_ValidId_ReturnTrue()
        {
            // Arrange
            _repoMock.Setup(r => r.SoftDeleteAsync(1)).Returns(Task.CompletedTask);
            _unitOfWorkMock
                .Setup(u => u.SaveAsync())
                .Returns(Task.FromResult(1));

            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            result.Should().BeTrue();
            _repoMock.Verify(r => r.SoftDeleteAsync(1), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }

        #endregion
    }
}
