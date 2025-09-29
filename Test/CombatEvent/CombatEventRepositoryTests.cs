using Domain.Entities;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.TestFixtures;

namespace Test.CombatEvents
{
    public class CombatEventRepositoryTests : IClassFixture<SqLiteInMemoryTestFixture>
    {
        private readonly SqLiteInMemoryTestFixture _fixture;
        public CombatEventRepositoryTests(SqLiteInMemoryTestFixture fixture) 
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCombatEvent_WhenExists()
        {
            using var context = _fixture.CreateIsolatedContext();
            var repo = new EfCombatEventRepository(context);

            var type = new EventType 
            {
                name = "Kill",
                category = "Combat",
                description = "Entity killed another entity"
            };

            context.EventType.Add(type);
            await context.SaveChangesAsync();

            var ev = new Event 
            {
                TimeStamp = DateTime.UtcNow,
                EventType = type
            };

            var actor = new Player 
            {
                GameIdentity = "A1",
                LastKnownName = "Actor"
            };

            var victim = new Player 
            {
                GameIdentity = "V1",
                LastKnownName = "Victim"
            };

            var combat = new CombatEvent
            {
                Event = ev,
                Actor = actor,
                Victim = victim,
            };

            await repo.AddAsync(combat);
            await repo.SaveChangesAsync();

            var loaded = await repo.GetByIdAsync(combat.EventId);

            Assert.NotNull(loaded);
            Assert.Equal("Actor", loaded.Actor.LastKnownName);
            Assert.Equal("Victim", loaded.Victim.LastKnownName);
            Assert.Equal("Kill", loaded.Event.EventType.name);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllCombatEventsInDescendingOrder()
        {
            using var context = _fixture.CreateIsolatedContext();
            var repo = new EfCombatEventRepository(context);

            var type = new EventType 
            {
                name = "Kill",
                category = "Combat",
                description = "Entity killed another entity"
            };

            context.EventType.Add(type);
            await context.SaveChangesAsync();

            for (int i = 0; i < 3; i++)
            {
                var ev = new Event 
                { 
                    TimeStamp = DateTime.UtcNow.AddMinutes(-i),
                    EventType = type
                };
                var actor = new Player 
                { 
                    GameIdentity = $"A{i}",
                    LastKnownName = $"Actor{i}"
                };
                var victim = new Player 
                {
                    GameIdentity = $"V{i}",
                    LastKnownName = $"Victim{i}" 
                };

                var combat = new CombatEvent
                {
                    Event = ev,
                    Actor = actor,
                    Victim = victim,
                };

                await repo.AddAsync(combat);
                await repo.SaveChangesAsync();
            }

            var all = (await repo.GetAllAsync()).ToList();

            Assert.Equal(3, all.Count);
            Assert.True(all[0].Event.TimeStamp > all[1].Event.TimeStamp); // descending
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
        {
            using var context = _fixture.CreateContext();
            var repo = new EfCombatEventRepository(context);

            var loaded = await repo.GetByIdAsync(999);

            Assert.Null(loaded);
        }


    }
}
