using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.TestFixtures;

namespace Test.Players
{
    public class PlayerRepositoryTests : IClassFixture<SqLiteInMemoryTestFixture>
    {
        private readonly SqLiteInMemoryTestFixture _fixture;
        public PlayerRepositoryTests(SqLiteInMemoryTestFixture fixture) 
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task CanGetByGameIdentity()
        {
            //Arrange, seed and save a player
            var context = _fixture.CreateIsolatedContext();
            var repo = new Infrastructure.Repositories.EfPlayerRepository(context);
            var player = new Domain.Entities.Player 
            { 
                GameIdentity = "player1", 
                LastKnownName = "TestPlayer" 
            };
            context.Player.Add(player);
            await context.SaveChangesAsync();

            //Act
            var loaded = await repo.GetByIdAsync("player1");

            //Assert
            Assert.NotNull(loaded);
            Assert.Equal("TestPlayer", loaded.LastKnownName);

        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
        {
            using var context = _fixture.CreateIsolatedContext();
            var repo = new Infrastructure.Repositories.EfPlayerRepository(context);
            var loaded = await repo.GetByIdAsync("nonexistent");
            Assert.Null(loaded);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllPlayers()
        {
            using var context = _fixture.CreateIsolatedContext();
            var repo = new Infrastructure.Repositories.EfPlayerRepository(context);
            var player1 = new Domain.Entities.Player 
            { 
                GameIdentity = "player1", 
                LastKnownName = "TestPlayer1" 
            };
            var player2 = new Domain.Entities.Player 
            { 
                GameIdentity = "player2", 
                LastKnownName = "TestPlayer2" 
            };
            context.Player.AddRange(player1, player2);
            await context.SaveChangesAsync();

            var loaded = await repo.GetAllAsync();

            Assert.Equal(2, loaded.Count());
            Assert.Contains(loaded, p => p.GameIdentity == "player1");
            Assert.Contains(loaded, p => p.GameIdentity == "player2");
        }
    }
}
