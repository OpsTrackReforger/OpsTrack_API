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

namespace Test.ConnectionEvents
{
    public class ConnectionEventServiceTests
    {
        private readonly Mock<IPlayerRepository> playerRepo = new();
        private readonly Mock<IEventRepository> eventRepo = new();
        private readonly Mock<IConnectionEventRepository> connRepo = new();

        [Fact]
        public async Task RegisterConnectionEventAsync_CreatesNewPlayer_WhenNotExists()
        {
            // Arrange
            playerRepo.Setup(r => r.GetByIdAsync("123")).ReturnsAsync((Player?)null);
            eventRepo.Setup(r => r.GetEventTypeByIdAsync(3))
                     .ReturnsAsync(new EventType { eventTypeId = 3, name = "Join" });

            var service = new ConnectionEventService(playerRepo.Object, eventRepo.Object, connRepo.Object);

            // Act
            var result = await service.RegisterConnectionEventAsync("123", "TestPlayer", ConnectionEventType.JOIN);

            // Assert
            Assert.Equal("123", result.GameIdentity);
            Assert.Equal("TestPlayer", result.Name);
            Assert.Equal("Join", result.EventType);

            playerRepo.Verify(r => r.AddAsync(It.Is<Player>(p => p.GameIdentity == "123")), Times.Once);
            playerRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
            eventRepo.Verify(r => r.AddAsync(It.IsAny<Event>()), Times.Once);
            connRepo.Verify(r => r.AddAsync(It.IsAny<ConnectionEvent>()), Times.Once);
        }

        [Fact]
        public async Task RegisterConnectionEventAsync_UpdatesExistingPlayer()
        {
            var existing = new Player { GameIdentity = "123", LastKnownName = "OldName" };
            playerRepo.Setup(r => r.GetByIdAsync("123")).ReturnsAsync(existing);
            eventRepo.Setup(r => r.GetEventTypeByIdAsync(4))
                     .ReturnsAsync(new EventType { eventTypeId = 4, name = "Leave" });

            var service = new ConnectionEventService(playerRepo.Object, eventRepo.Object, connRepo.Object);

            var result = await service.RegisterConnectionEventAsync("123", "NewName", ConnectionEventType.LEAVE);

            Assert.Equal("NewName", result.Name);
            Assert.Equal("Leave", result.EventType);

            playerRepo.Verify(r => r.UpdateAsync(It.Is<Player>(p => p.LastKnownName == "NewName")), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_MapsEntitiesToDtos()
        {
            var events = new List<ConnectionEvent>
        {
            new ConnectionEvent
            {
                EventId = 1,
                GameIdentity = "123",
                Name = "Tester",
                Event = new Event
                {
                    TimeStamp = new DateTime(2025, 10, 1),
                    EventType = new EventType { name = "Join" }
                }
            }
        };

            connRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(events);

            var service = new ConnectionEventService(playerRepo.Object, eventRepo.Object, connRepo.Object);

            var result = (await service.GetAllAsync()).ToList();

            Assert.Single(result);
            Assert.Equal("123", result[0].GameIdentity);
            Assert.Equal("Join", result[0].EventType);
            Assert.Equal(new DateTime(2025, 10, 1), result[0].Timestamp);
        }


    }
}
