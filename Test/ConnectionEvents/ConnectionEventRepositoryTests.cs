using Domain.Entities;
using Infrastructure.Repositories;
using Test.TestFixtures;
using Xunit;

namespace OpsTrack.Test.ConnectionEvents;

public class ConnectionEventRepositoryTests : IClassFixture<SqLiteInMemoryTestFixture>
{
    private readonly SqLiteInMemoryTestFixture _fixture;

    public ConnectionEventRepositoryTests(SqLiteInMemoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task CanAddAndRetrieveByIdConnectionEvent()
    {
        // Arrange
        var context = _fixture.Context;
        var repo = new EfConnectionEventRepository(context);

        var eventType = new EventType 
        { 
            name = "Join",
            category = "Connection",
            description = "Player joined the game"
        };

        context.EventType.Add(eventType);
        await context.SaveChangesAsync();

        var ev = new Event 
        {
            TimeStamp = DateTime.UtcNow,
            EventType = eventType
        };

        context.Event.Add(ev);
        await context.SaveChangesAsync();

        var player = new Player 
        { 
            GameIdentity = "1234", 
            LastKnownName = "TestPlayer" 
        };

        var conn = new ConnectionEvent 
        { 
            Name = "TestPlayer",
            Event = ev,
            Player = player
        };
        
        // Act
        await repo.AddAsync(conn);
        await repo.SaveChangesAsync();

        var loaded = await repo.GetByIdAsync(conn.EventId);

        // Assert
        Assert.NotNull(loaded);
        Assert.Equal("1234", loaded.GameIdentity);
        Assert.Equal("Join", loaded.Event.EventType.name);
    }
}
