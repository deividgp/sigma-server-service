using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Services;
using Moq;

namespace Tests
{
    public class ServerServiceTests
    {
        private readonly Mock<IRepository<Server, Guid>> _mockServerRepository;
        private readonly ServerService _serverService;

        public ServerServiceTests()
        {
            _mockServerRepository = new Mock<IRepository<Server, Guid>>();
            _serverService = new ServerService(_mockServerRepository.Object);
        }

        [Fact]
        public async Task GetServer_WithValidServerId_ReturnsServer()
        {
            // Arrange
            var serverId = Guid.NewGuid();
            var expectedServer = new Server { Id = serverId, Name = "Test Server" };
            _mockServerRepository
                .Setup(repo => repo.GetByIdAsync(serverId))
                .ReturnsAsync(expectedServer);

            // Act
            var result = await _serverService.GetServer(serverId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(serverId, result.Id);
        }

        [Fact]
        public async Task GetServer_WithInvalidServerId_ReturnsNull()
        {
            // Arrange
            var serverId = Guid.NewGuid();
            _mockServerRepository
                .Setup(repo => repo.GetByIdAsync(serverId))
                .ReturnsAsync((Server)null);

            // Act
            var result = await _serverService.GetServer(serverId);

            // Assert
            Assert.Null(result);
        }
    }
}
