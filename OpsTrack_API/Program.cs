using Application.Dtos;
using Application.Enums;
using Application.Services;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext with SQLite
builder.Services.AddDbContext<OpsTrackContext>(options =>
    options.UseSqlite("Data Source=opstrack.db"));

//Add repositories and services
builder.Services.AddScoped<IPlayerRepository, EfPlayerRepository>();
builder.Services.AddScoped<IConnectionEventRepository, EfConnectionEventRepository>();
builder.Services.AddScoped<IConnectionEventService, ConnectionEventService>();
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
app.MapPost("/player/join", async (PlayerEventRequest req, IConnectionEventService service) =>
{
    var result = await service.RegisterConnectionEventAsync(
        req.GameIdentity,
        req.Name,
        ConnectionEventType.JOIN
    );
    return Results.Ok(result);
});

app.MapPost("/player/leave", async (PlayerEventRequest req, IConnectionEventService service) =>
{
    var result = await service.RegisterConnectionEventAsync(
        req.GameIdentity,
        req.Name,
        ConnectionEventType.LEAVE
    );
    return Results.Ok(result);
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


app.Run();