using Microsoft.EntityFrameworkCore;
using OpsTrack_API.Data;
using OpsTrack_API.Models;
using OpsTrack_API.Data;
using OpsTrack_API.Models;

var builder = WebApplication.CreateBuilder(args);

// Tilføj services til Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Tilføj DbContext (som du allerede har)
builder.Services.AddDbContext<OpsTrackContext>(options =>
    options.UseSqlite("Data Source=opstrack.db"));

var app = builder.Build();

// Slå Swagger til i Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Dine endpoints
app.MapPost("/player/join", async (PlayerEvent ev, OpsTrackContext db) =>
{
    ev.EventType = "join";
    ev.Timestamp = DateTime.UtcNow;
    db.PlayerEvents.Add(ev);
    await db.SaveChangesAsync();
    return Results.Ok(ev);
});

app.MapPost("/player/leave", async (PlayerEvent ev, OpsTrackContext db) =>
{
    ev.EventType = "leave";
    ev.Timestamp = DateTime.UtcNow;
    db.PlayerEvents.Add(ev);
    await db.SaveChangesAsync();
    return Results.Ok(ev);
});

app.MapGet("/events", async (OpsTrackContext db) =>
    await db.PlayerEvents.ToListAsync());

app.Run();