using Microsoft.EntityFrameworkCore;
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
app.MapPost("/player/join", async (ConnectionEvent ev, OpsTrackContext db) =>
{
    ev.EventType = "join";
    ev.Timestamp = DateTime.UtcNow;
    db.ConnectionEvents.Add(ev);
    await db.SaveChangesAsync();
    return Results.Ok(ev);
});

app.MapPost("/player/leave", async (ConnectionEvent ev, OpsTrackContext db) =>
{
    ev.EventType = "leave";
    ev.Timestamp = DateTime.UtcNow;
    db.ConnectionEvents.Add(ev);
    await db.SaveChangesAsync();
    return Results.Ok(ev);
});

app.MapGet("/events", async (OpsTrackContext db) =>
    await db.ConnectionEvents.ToListAsync());

app.Run();