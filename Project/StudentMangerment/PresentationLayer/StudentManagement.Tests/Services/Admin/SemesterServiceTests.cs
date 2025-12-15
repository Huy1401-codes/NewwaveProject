using BusinessLogicLayer.DTOs.Admin;
using BusinessLogicLayer.Messages.Admin;
using BusinessLogicLayer.Services.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace StudentManagement.Tests.Services.Admin
{
    public class SemesterServiceTests
    {
        private readonly Mock<ISemesterRepository> _repoMock;
        private readonly Mock<ILogger<SemesterService>> _loggerMock;
        private readonly SemesterService _service;

        public SemesterServiceTests()
        {
            _repoMock = new Mock<ISemesterRepository>();
            _loggerMock = new Mock<ILogger<SemesterService>>();

            _service = new SemesterService(
                _repoMock.Object,
                _loggerMock.Object
            );
        }

        #region Add
        [Fact]
        public async Task AddAsync_StartDateGreaterThanEndDate_ThrowArgumentException()
        {
            var dto = new SemesterCreateDto
            {
                Name = "HK1",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(-1)
            };

            // Act
            Func<Task> act = async () => await _service.AddAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage(SemesterMessages.InvalidDate);
        }

        [Fact]
        public async Task AddAsync_DuplicateTimeRange_ThrowArgumentException()
        {
            var dto = new SemesterCreateDto
            {
                Name = "HK1",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(3)
            };

            _repoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Semester, bool>>>()))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _service.AddAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage(SemesterMessages.DuplicateInTimeRange);
        }

        [Fact]
        public async Task AddAsync_ValidData_AddSuccessfully()
        {
            // Arrange: Dữ liệu hợp lệ
            var dto = new SemesterCreateDto
            {
                Name = "HK1",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(3)
            };

            _repoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Semester, bool>>>()))
                .ReturnsAsync(false);

            // Act
            Func<Task> act = async () => await _service.AddAsync(dto);

            // Assert
            await act.Should().NotThrowAsync();
            _repoMock.Verify(r => r.AddAsync(It.IsAny<Semester>()), Times.Once);
        }
        #endregion


        #region Update
        [Fact]
        public async Task UpdateAsync_SemesterNotFound_DoNothing()
        {
            // Arrange: Semester không tồn tại
            var dto = new SemesterDto
            {
                SemesterId = 1,
                Name = "HK1",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(3)
            };

            _repoMock.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync((Semester)null);

            // Act
            Func<Task> act = async () => await _service.UpdateAsync(dto);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task UpdateAsync_DuplicateTimeRange_ThrowArgumentException()
        {
            // Arrange: Trùng khoảng thời gian khi update
            var dto = new SemesterDto
            {
                SemesterId = 1,
                Name = "HK1",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(3)
            };

            _repoMock.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(new Semester { Id = 1 });

            _repoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Semester, bool>>>()))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _service.UpdateAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage(SemesterMessages.DuplicateInTimeRange);
        }

        [Fact]
        public async Task UpdateAsync_ValidData_UpdateSuccessfully()
        {
            // Arrange
            var semester = new Semester { Id = 1 };

            var dto = new SemesterDto
            {
                SemesterId = 1,
                Name = "HK1",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(3)
            };

            _repoMock.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(semester);

            _repoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Semester, bool>>>()))
                .ReturnsAsync(false);

            // Act
            Func<Task> act = async () => await _service.UpdateAsync(dto);

            // Assert
            await act.Should().NotThrowAsync();
            _repoMock.Verify(r => r.UpdateAsync(semester), Times.Once);
        }
        #endregion

    }
}
