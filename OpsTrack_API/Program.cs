using Microsoft.EntityFrameworkCore;
using OpsTrack_API.Data;
using OpsTrack_API.Models;
using OpsTrack_API.Data;
using OpsTrack_API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext with SQLite
builder.Services.AddDbContext<OpsTrackContext>(options =>
    options.UseSqlite("Data Source=opstrack.db"));

var app = builder.Build();

// Enable swagger (Should it only be enabled in development?)
app.UseSwagger();
    app.UseSwaggerUI();

// End points

// Player join event
app.MapPost("/player/join", async (PlayerEvent ev, OpsTrackContext db) =>
{
    ev.EventType = "join";
    ev.Timestamp = DateTime.UtcNow;
    db.PlayerEvents.Add(ev);
    await db.SaveChangesAsync();
    return Results.Ok(ev);
});

// Player leave event
app.MapPost("/player/leave", async (PlayerEvent ev, OpsTrackContext db) =>
{
    ev.EventType = "leave";
    ev.Timestamp = DateTime.UtcNow;
    db.PlayerEvents.Add(ev);
    await db.SaveChangesAsync();
    return Results.Ok(ev);
});

// Get all events
app.MapGet("/events", async (OpsTrackContext db) =>
    await db.PlayerEvents.ToListAsync());

app.Run();