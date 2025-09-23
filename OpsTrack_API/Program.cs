using Application.Dtos;
using Application.Enums;
using Application.Services;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var solutionRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\.."));
// Dette giver: C:\Users\Christopher\source\repos\OpsTrackReforger\OpsTrack_API

var dataPath = Path.Combine(solutionRoot, "Data");
Directory.CreateDirectory(dataPath);

var connectionString = $"Data Source={Path.Combine(dataPath, "opstrack.db")}";


// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext with SQLite
builder.Services.AddDbContext<OpsTrackContext>(options =>
    options.UseSqlite(connectionString));

//Add repositories and services
builder.Services.AddScoped<IPlayerRepository, EfPlayerRepository>();
builder.Services.AddScoped<IConnectionEventRepository, EfConnectionEventRepository>();
builder.Services.AddScoped<IConnectionEventService, ConnectionEventService>();
builder.Services.AddControllers();
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

app.MapControllers();

app.Run();