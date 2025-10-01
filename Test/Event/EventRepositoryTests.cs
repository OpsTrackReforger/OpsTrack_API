using Domain.Entities;
using Infrastructure.Repositories;
using Test.TestFixtures;

namespace Test.Events
{
    public class EventRepositoryTests : IClassFixture<SqLiteInMemoryTestFixture>
    {
        private readonly SqLiteInMemoryTestFixture _fixture;

        public EventRepositoryTests(SqLiteInMemoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsEvent_WhenExists()
        {
            using var context = _fixture.CreateIsolatedContext();
            var repo = new EfEventRepository(context);

            var type = new EventType 
            {
                name = "Join",
                category = "Connection",
                description = "Player joined the game"
            };

            context.EventType.Add(type);
            await context.SaveChangesAsync();

            var ev = new Event 
            {
                TimeStamp = DateTime.UtcNow,
                EventType = type
            };

            context.Event.Add(ev);
            await context.SaveChangesAsync();

            var loaded = await repo.GetByIdAsync(ev.EventId);

            Assert.NotNull(loaded);
            Assert.Equal("Join", loaded.EventType.name);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllEvents()
        {
            using var context = _fixture.CreateIsolatedContext();
            var repo = new EfEventRepository(context);

            var type = new EventType 
            {
                name = "Join",
                category = "Connection",
                description = "Player joined the game"
            };

            context.EventType.Add(type);
            await context.SaveChangesAsync();

            context.Event.AddRange(
                new Event 
                { 
                    TimeStamp = DateTime.UtcNow.AddMinutes(-1),
                    EventType = type
                },
                new Event 
                {
                    TimeStamp = DateTime.UtcNow,
                    EventType = type 
                }
            );

            await context.SaveChangesAsync();

            var all = (await repo.GetAllAsync()).ToList();

            Assert.Equal(2, all.Count);
        }

        [Fact]
        public async Task GetByTypeAsync_ReturnsOnlyMatchingEvents()
        {
            using var context = _fixture.CreateIsolatedContext();
            var repo = new EfEventRepository(context);

            var joinType = new EventType 
            {
                name = "Join",
                category = "Connection",
                description = "Player joined the game"
            };

            var leaveType = new EventType 
            { 
                name = "Leave",
                category = "Connection",
                description = "Player left the game"
            };

            context.EventType.AddRange(joinType, leaveType);
            await context.SaveChangesAsync();

            context.Event.AddRange(
                new Event 
                { 
                    TimeStamp = DateTime.UtcNow,
                    EventType = joinType 
                },
                new Event 
                { 
                    TimeStamp = DateTime.UtcNow,
                    EventType = leaveType
                }
            );
            await context.SaveChangesAsync();

            var joinEvents = (await repo.GetByTypeAsync(joinType.eventTypeId)).ToList();

            Assert.Single(joinEvents);
            Assert.Equal("Join", joinEvents[0].EventType.name);
        }

        [Fact]
        public async Task AddAndSaveChanges_PersistsEvent()
        {
            using var context = _fixture.CreateIsolatedContext();
            var repo = new EfEventRepository(context);

            var type = new EventType 
            {
                name = "Join",
                category = "Connection",
                description = "Player joined the game"
            };

            context.EventType.Add(type);
            await context.SaveChangesAsync();

            var ev = new Event 
            { 
                TimeStamp = DateTime.UtcNow,
                EventType = type 
            };

            await repo.AddAsync(ev);
            await repo.SaveChangesAsync();

            var loaded = await repo.GetByIdAsync(ev.EventId);

            Assert.NotNull(loaded);
            Assert.Equal(type.eventTypeId, loaded.EventTypeId);
        }

        [Fact]
        public async Task GetEventTypeByIdAsync_ReturnsEventType()
        {
            using var context = _fixture.CreateIsolatedContext();
            var repo = new EfEventRepository(context);

            var type = new EventType 
            {
                name = "Join",
                category = "Connection",
                description = "Player joined the game"
            };

            context.EventType.Add(type);
            await context.SaveChangesAsync();

            var loaded = await repo.GetEventTypeByIdAsync(type.eventTypeId);

            Assert.NotNull(loaded);
            Assert.Equal("Join", loaded.name);
        }

    }

}
