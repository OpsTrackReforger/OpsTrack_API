using Application.Enums;
using Application.Services;
using Domain.Entities;
using Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Players
{
    public class PlayerServiceTests
    {
        private readonly Mock<IPlayerRepository> playerRepo = new();
        private readonly Mock<IConnectionEventRepository> connRepo = new();

        [Fact]
        public async Task GetAllAsync_ReturnsMappedPlayers()
        {
            // Arrange
            var players = new List<Player>
        {
            new Player { GameIdentity = "123", LastKnownName = "Alice", FirstSeen = DateTime.UtcNow.AddDays(-5), LastSeen = DateTime.UtcNow }
        };
            playerRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(players);

            var service = new PlayerService(playerRepo.Object, connRepo.Object);

            // Act
            var result = (await service.GetAllAsync()).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal("123", result[0].GameIdentity);
            Assert.Equal("Alice", result[0].LastKnownName);
        }

        [Fact]
        public async Task GetOnline_ReturnsOnlyPlayersWithNonLeaveEvents()
        {
            // Arrange
            var joinEvent = new ConnectionEvent
            {
                GameIdentity = "123",
                Name = "Alice",
                Event = new Event { EventTypeId = (int)ConnectionEventType.JOIN, TimeStamp = DateTime.UtcNow }
            };

            var leaveEvent = new ConnectionEvent
            {
                GameIdentity = "456",
                Name = "Bob",
                Event = new Event { EventTypeId = (int)ConnectionEventType.LEAVE, TimeStamp = DateTime.UtcNow }
            };

            connRepo.Setup(r => r.GetLatestEventsByPlayerAsync())
                    .ReturnsAsync(new List<ConnectionEvent?> { joinEvent, leaveEvent });

            var service = new PlayerService(playerRepo.Object, connRepo.Object);

            // Act
            var result = (await service.GetOnline()).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal("123", result[0].GameIdentity);
            Assert.Equal("Alice", result[0].LastKnownName);
        }

        [Fact]
        public async Task GetOnline_UsesEventTimestamp_WhenPlayerIsNull()
        {
            // Arrange
            var ev = new Event { EventTypeId = (int)ConnectionEventType.JOIN, TimeStamp = new DateTime(2025, 10, 1) };
            var connEvent = new ConnectionEvent
            {
                GameIdentity = "789",
                Name = "Charlie",
                Event = ev,
                Player = null // simulerer at Player ikke er loaded
            };

            connRepo.Setup(r => r.GetLatestEventsByPlayerAsync())
                    .ReturnsAsync(new List<ConnectionEvent?> { connEvent });

            var service = new PlayerService(playerRepo.Object, connRepo.Object);

            // Act
            var result = (await service.GetOnline()).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal(new DateTime(2025, 10, 1), result[0].FirstSeen);
            Assert.Equal(new DateTime(2025, 10, 1), result[0].LastSeen);
        }
    }

}
