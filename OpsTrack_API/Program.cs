using Application.Dtos;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpsTrack_API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext with SQLite
builder.Services.AddDbContext<OpsTrackContext>(options =>
    options.UseSqlite("Data Source=opstrack.db"));

var app = builder.Build();
app.UseDeveloperExceptionPage();

// Run migrations at startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OpsTrackContext>();
    db.Database.Migrate();
}

// Enable swagger
app.UseSwagger();
app.UseSwaggerUI();

// Endpoints
// POST: /player/join
app.MapPost("/player/join", async (PlayerEventRequest req, OpsTrackContext db) =>
{
    var ev = new ConnectionEvent
    {
        GameIdentity = req.GameIdentity,
        Name = req.Name,
        EventType = "join",
        Timestamp = DateTime.UtcNow
    };

    // Find eller opret Player
    var player = await db.Players.FindAsync(ev.GameIdentity);
    if (player == null)
    {
        player = new Player
        {
            GameIdentity = ev.GameIdentity,
            LastKnownName = ev.Name,
            FirstSeen = ev.Timestamp,
            LastSeen = ev.Timestamp
        };
        db.Players.Add(player);
    }
    else
    {
        player.LastKnownName = ev.Name;
        player.LastSeen = ev.Timestamp;
    }

    db.ConnectionEvents.Add(ev);
    await db.SaveChangesAsync();

    return Results.Ok(new ConnectionEventResponse(
            ev.EventId,
            ev.GameIdentity,
            ev.Name,
            ev.EventType,
            ev.Timestamp
        ));
});



// POST: /player/leave
app.MapPost("/player/leave", async (PlayerEventRequest req, OpsTrackContext db) =>
{
    var ev = new ConnectionEvent()
    {
        GameIdentity = req.GameIdentity,
        Name = req.Name,
        EventType = "leave",
        Timestamp = DateTime.UtcNow
    };

    var player = await db.Players.FindAsync(ev.GameIdentity);
    if (player != null)
    {
        player.LastSeen = ev.Timestamp;
    }

    db.ConnectionEvents.Add(ev);
    await db.SaveChangesAsync();

    return Results.Ok(new ConnectionEventResponse(
        ev.EventId,
        ev.GameIdentity,
        ev.Name,
        ev.EventType,
        ev.Timestamp
    ));
});

//GET: /events/connections
app.MapGet("/events/connections", async (OpsTrackContext db) =>
    await db.ConnectionEvents
        .AsNoTracking()
        .OrderByDescending(e => e.Timestamp)
        .Select(e => new ConnectionEventResponse(
            e.EventId,
            e.GameIdentity,
            e.Name,
            e.EventType,
            e.Timestamp
        ))
        .ToListAsync());


//GET: /events/connections/{id}
app.MapGet("/events/connections/{id}", async (string id, OpsTrackContext db) =>
    await db.ConnectionEvents
        .Where(e => e.GameIdentity == id)
        .Include(e => e.Player)
        .AsNoTracking()
        .Select(e => new ConnectionEventResponse(
            e.EventId,
            e.GameIdentity,
            e.Name,
            e.EventType,
            e.Timestamp
        ))
        .OrderByDescending(e => e.Timestamp)
        .ToListAsync());

//GET: /events/connections/latest/{count}
app.MapGet("/events/connections/latest/{count:int}", async (int count, OpsTrackContext db) =>
    await db.ConnectionEvents
        .OrderByDescending(e => e.Timestamp)
        .Include(e => e.Player)
        .AsNoTracking()
        .Take(count)
        .Select(e => new ConnectionEventResponse(
            e.EventId,
            e.GameIdentity,
            e.Name,
            e.EventType,
            e.Timestamp
        ))
        .ToListAsync());


// GET: /players
app.MapGet("/players", async (OpsTrackContext db) =>
    await db.Players
    .AsNoTracking()
    .Select(p => new PlayerResponse(
        p.GameIdentity,
        p.LastKnownName,
        p.FirstSeen,
        p.LastSeen
    ))
    .ToListAsync());

// GET: /players/{id}
app.MapGet("/players/{id}", async (string id, OpsTrackContext db) =>
{
    var player = await db.Players
        .AsNoTracking()
        .FirstOrDefaultAsync(p => p.GameIdentity == id);

    return player is not null
        ? Results.Ok(new PlayerResponse(
            player.GameIdentity,
            player.LastKnownName,
            player.FirstSeen,
            player.LastSeen
          ))
        : Results.NotFound();
});



app.Run();