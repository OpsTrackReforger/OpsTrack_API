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
    public async Task CanAddAndRetrieveByEventIdConnectionEvent()
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

    [Fact]
    public void CanAddAndRetrieveByPlayerGameIdentityConnectionEvent()
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
        context.SaveChanges();
        var ev = new Event 
        {
            TimeStamp = DateTime.UtcNow,
            EventType = eventType
        };
        context.Event.Add(ev);
        context.SaveChanges();
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
        repo.AddAsync(conn).Wait();
        repo.SaveChangesAsync().Wait();
        var loadedList = repo.GetByPlayerGameIdentityAsync("1234").Result;
        var loaded = loadedList.FirstOrDefault();
        // Assert
        Assert.NotNull(loaded);
        Assert.Equal("1234", loaded.GameIdentity);
        Assert.Equal("Join", loaded.Event.EventType.name);
    }

    [Fact]
    public void CanAddAndRetrieveLatestConnectionEvents()
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
        context.SaveChanges();

        for (int i = 0; i < 5; i++)
        {
            var ev = new Event 
            {
                TimeStamp = DateTime.UtcNow.AddMinutes(-i),
                EventType = eventType
            };
            context.Event.Add(ev);
            context.SaveChanges();
            var player = new Player 
            { 
                GameIdentity = (1234 + i).ToString(), 
                LastKnownName = "TestPlayer" + i 
            };
            var conn = new ConnectionEvent 
            { 
                Name = "TestPlayer" + i,
                Event = ev,
                Player = player
            };
            repo.AddAsync(conn).Wait();
            repo.SaveChangesAsync().Wait();
        }

        // Act
        var latestThree = repo.GetLatestAsync(3).Result.ToList();
        // Assert
        Assert.Equal(3, latestThree.Count);
        Assert.Equal("TestPlayer0", latestThree[0].Name); // Most recent
        Assert.Equal("TestPlayer1", latestThree[1].Name);
        Assert.Equal("TestPlayer2", latestThree[2].Name);
    }


}
