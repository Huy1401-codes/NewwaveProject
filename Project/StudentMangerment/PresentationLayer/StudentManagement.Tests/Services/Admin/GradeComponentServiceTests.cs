using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace StudentManagement.Tests.Services.Admin
{
    public class GradeComponentServiceTests
    {
        private readonly Mock<IGradeComponentRepository> _repoMock;
        private readonly Mock<ILogger<GradeComponentService>> _loggerMock;
        private readonly GradeComponentService _service;

        public GradeComponentServiceTests()
        {
            _repoMock = new Mock<IGradeComponentRepository>();
            _loggerMock = new Mock<ILogger<GradeComponentService>>();

            _service = new GradeComponentService(
                _repoMock.Object,
                _loggerMock.Object
            );
        }
        #region Add
        [Fact]
        public async Task AddComponent_ValidDto_AddSuccessfully()
        {
            // Arrange
            var dto = new CreateGradeComponentDto
            {
                SubjectId = 1,
                ComponentName = "Quiz",
                Weight = 20
            };

            // Act
            Func<Task> act = async () => await _service.AddComponent(dto);

            // Assert
            await act.Should().NotThrowAsync();
        }
        #endregion

        #region Update
        [Fact]
        public async Task UpdateComponent_ComponentNotFound_DoNothing()
        {

            // Arrange
            var id = 1;
            var dto = new UpdateGradeComponentDto
            {
                ComponentName = "Updated",
                Weight = 30
            };

            _repoMock
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync((GradeComponent)null);

            // Act
            Func<Task> act = async () => await _service.UpdateComponent(id, dto);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task UpdateComponent_ValidComponent_UpdateSuccessfully()
        {

            // Arrange
            var id = 1;
            var entity = new GradeComponent
            {
                Id = id,
                ComponentName = "Old",
                Weight = 10
            };

            var dto = new UpdateGradeComponentDto
            {
                ComponentName = "New",
                Weight = 40
            };

            _repoMock
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(entity);

            // Act
            Func<Task> act = async () => await _service.UpdateComponent(id, dto);

            // Assert
            await act.Should().NotThrowAsync();
            entity.ComponentName.Should().Be("New");
            entity.Weight.Should().Be(40);
        }
        #endregion

        #region Delete
        [Fact]
        public async Task DeleteComponent_ValidId_DeleteSuccessfully()
        {

            // Arrange
            var id = 1;

            // Act
            Func<Task> act = async () => await _service.DeleteComponent(id);

            // Assert
            await act.Should().NotThrowAsync();
        }
        #endregion

    }
}
