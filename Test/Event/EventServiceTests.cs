using Domain.Entities;
using Domain.Repositories;
using Moq;
using Application.Services;


namespace Test.Events
{
    public class EventServiceTests
    {

        [Fact]
        public async Task GetByIdAsync_ReturnsEvent_WhenExists()
        {
            // Arrange
            var repoMock= new Mock<IEventRepository>();
            int expectedEventId = 1;
            var expectedEvent = new Event
            {
                EventId = expectedEventId,
                TimeStamp = DateTime.UtcNow,
                EventTypeId = 2,
                EventType = new EventType 
                {
                    eventTypeId = 2,
                    name = "TestEvent",
                    category = "TestCategory",
                    description = "A test event type" 
                }
            };

            var service = new EventService(repoMock.Object);
            repoMock.Setup(r => r.GetByIdAsync(expectedEventId)).ReturnsAsync(expectedEvent);

            // Act
            var result = await service.GetByIdAsync(expectedEventId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedEventId, result.EventId);
            repoMock.Verify(r => r.GetByIdAsync(expectedEventId), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
        {
            // Arrange
            var repoMock= new Mock<IEventRepository>();
            int nonExistentEventId = 999;
            var service = new EventService(repoMock.Object);
            repoMock.Setup(r => r.GetByIdAsync(nonExistentEventId)).ReturnsAsync((Event?)null);
            
            // Act
            var result = await service.GetByIdAsync(nonExistentEventId);
            
            // Assert
            Assert.Null(result);
            repoMock.Verify(r => r.GetByIdAsync(nonExistentEventId), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllEvents()
        {
            // Arrange
            var repoMock= new Mock<IEventRepository>();
            var expectedEvents = new List<Event>
            {
                new Event { EventId = 1,
                    TimeStamp = DateTime.UtcNow,
                    EventTypeId = 2 },
                new Event { EventId = 2,
                    TimeStamp = DateTime.UtcNow.AddMinutes(-10),
                    EventTypeId = 3 }
            };
            var service = new EventService(repoMock.Object);
            repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(expectedEvents);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            repoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByTypeAsync_ReturnsFilteredEvents()
        {
            // Arrange
            var repoMock= new Mock<IEventRepository>();
            int filterEventTypeId = 2;
            var expectedEvents = new List<Event>
            {
                new Event 
                {
                    EventId = 1,
                    TimeStamp = DateTime.UtcNow,
                    EventType = new EventType 
                    {
                        category = "TestCategory",
                        description = "A test event type",
                        name = "TestEvent",
                        eventTypeId = filterEventTypeId
                    }
                },
                new Event 
                { 
                    EventId = 3,
                    TimeStamp = DateTime.UtcNow.AddMinutes(-5),
                    EventType = new EventType
                    {
                        category = "TestCategory",
                        description = "A test event type",
                        name = "TestEvent",
                        eventTypeId = filterEventTypeId
                    }
                }
            };
            var service = new EventService(repoMock.Object);
            repoMock.Setup(r => r.GetByTypeAsync(filterEventTypeId)).ReturnsAsync(expectedEvents);
            
            // Act
            var result = await service.GetByTypeAsync(filterEventTypeId);
            
            // Assert
            Assert.NotNull(result);
            Assert.All(result, e => Assert.Equal(filterEventTypeId, e.EventType.eventTypeId));
            repoMock.Verify(r => r.GetByTypeAsync(filterEventTypeId), Times.Once);
        }

        [Fact]
        public async Task RegisterEventAsync_AddsAndSavesEvent()
        {
            // Arrange
            var repoMock= new Mock<IEventRepository>();
            var newEvent = new Event
            {
                TimeStamp = DateTime.UtcNow,
                EventTypeId = 2,
                EventType = new EventType 
                { 
                    eventTypeId = 2,
                    name = "TestEvent",
                    category = "TestCategory",
                    description = "A test event type"
                }
            };
            var service = new EventService(repoMock.Object);
            repoMock.Setup(r => r.AddAsync(newEvent)).Returns(Task.CompletedTask);
            repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await service.RegisterEventAsync(newEvent);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newEvent, result);
            repoMock.Verify(r => r.AddAsync(newEvent), Times.Once);
            repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

    }
}
