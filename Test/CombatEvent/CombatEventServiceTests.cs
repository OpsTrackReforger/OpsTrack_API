using Application.Dtos;
using Application.Services;
using Domain.Entities;
using Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.CombatEvents
{
    public class CombatEventServiceTests
    {
        private readonly Mock<ICombatEventRepository> combatRepo = new();
        private readonly Mock<IEventRepository> eventRepo = new();

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            combatRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((CombatEvent?)null);
            var service = new CombatEventService(combatRepo.Object, eventRepo.Object);

            var result = await service.GetByIdAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_MapsEntityToDto()
        {
            var ev = new Event
            {
                EventId = 1,
                TimeStamp = new DateTime(2025, 10, 1),
                EventType = new EventType { name = "Kill" }
            };
            var combat = new CombatEvent
            {
                EventId = 1,
                Event = ev,
                ActorId = "A1",
                Actor = new Player { LastKnownName = "ActorName" },
                VictimId = "V1",
                Victim = new Player { LastKnownName = "VictimName" },
                Weapon = "Rifle",
                Distance = 123,
                IsTeamKill = false
            };

            combatRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(combat);
            var service = new CombatEventService(combatRepo.Object, eventRepo.Object);

            var dto = await service.GetByIdAsync(1);

            Assert.NotNull(dto);
            Assert.Equal("ActorName", dto.ActorName);
            Assert.Equal("VictimName", dto.VictimName);
            Assert.Equal("Kill", dto.EventTypeName);
            Assert.Equal("Rifle", dto.Weapon);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedDtos()
        {
            var events = new List<CombatEvent>
        {
            new CombatEvent
            {
                EventId = 1,
                Event = new Event
                {
                    TimeStamp = DateTime.UtcNow,
                    EventType = new EventType { name = "Kill" }
                },
                ActorId = "A1",
                VictimId = "V1"
            }
        };

            combatRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(events);
            var service = new CombatEventService(combatRepo.Object, eventRepo.Object);

            var result = (await service.GetAllAsync()).ToList();

            Assert.Single(result);
            Assert.Equal("Kill", result[0].EventTypeName);
        }

        [Fact]
        public async Task RegisterCombatEventAsync_CreatesEventAndCombatEvent()
        {
            var req = new CombatEventRequest
            (
                EventTypeId: 99,
                ActorId: "A1",
                ActorName: "Actor",
                ActorFaction: "FactionA",
                VictimFaction: "FactionB",
                VictimName: "Victim",
                VictimId: "V1",
                Weapon: "Sniper",
                Distance: 200,
                IsTeamKill: true
            );

            eventRepo.Setup(r => r.GetEventTypeByIdAsync(99))
                     .ReturnsAsync(new EventType { eventTypeId = 99, name = "Headshot", description = "SOme description" });

            var service = new CombatEventService(combatRepo.Object, eventRepo.Object);

            var result = await service.RegisterCombatEventAsync(req);

            Assert.Equal("A1", result.ActorId);
            Assert.Equal("V1", result.VictimId);
            Assert.Equal("Sniper", result.Weapon);
            Assert.Equal("Headshot", result.EventTypeName);

            eventRepo.Verify(r => r.AddAsync(It.IsAny<Event>()), Times.Once);
            eventRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
            combatRepo.Verify(r => r.AddAsync(It.IsAny<CombatEvent>()), Times.Once);
            combatRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetCombatEventsByDate_ReturnsListWithinRange()
        {
            var start = new DateTime(2025, 1, 1);
            var end = new DateTime(2025, 12, 31);
            var events = new List<CombatEvent>
            {
                new CombatEvent
                {
                    EventId = 1,
                    Event = new Event
                    {
                        TimeStamp = new DateTime(2025, 6, 15),
                        EventType = new EventType { name = "Kill" }
                    },
                    ActorId = "A1",
                    VictimId = "V1"
                }
            };
            combatRepo.Setup(r => r.GetByDateRangeAsync(start, end)).ReturnsAsync(events);
            var service = new CombatEventService(combatRepo.Object, eventRepo.Object);
            var result = (await service.GetByDateRangeAsync(start, end)).ToList();
            Assert.Single(result);
            Assert.Equal("Kill", result[0].EventTypeName);
        }
    }

}
