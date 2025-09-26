using Application.Services;
using Domain.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpsTrack_API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Bind Kestrel til alle interfaces (docker)
builder.WebHost.UseUrls("http://+:8080");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Hent API nøgle fra miljøvariabel
var apiKey = builder.Configuration["ApiKey"] ?? throw new InvalidOperationException("API key not set in environment variables.");


// Database provider
var provider = builder.Configuration["DatabaseProvider"];
if (provider == "MySql")
{
    var cs = builder.Configuration.GetConnectionString("MySql");
    builder.Services.AddDbContext<OpsTrackContext>(options =>
        options.UseMySql(cs, ServerVersion.AutoDetect(cs)));
}
else if (provider == "SqlServer")
{
    var cs = builder.Configuration.GetConnectionString("SqlServer");
    builder.Services.AddDbContext<OpsTrackContext>(options =>
        options.UseSqlServer(cs));
}
else
{
    // Docker volume path
    var dataPath = Path.Combine(AppContext.BaseDirectory, "Data");
    Directory.CreateDirectory(dataPath);
    var connectionString = $"Data Source={Path.Combine(dataPath, "opstrack.db")}";
    builder.Services.AddDbContext<OpsTrackContext>(options =>
        options.UseSqlite(connectionString));
}

// Add repositories and services
builder.Services.AddScoped<IEventRepository, EfEventRepository>();
builder.Services.AddScoped<IPlayerRepository, EfPlayerRepository>();
builder.Services.AddScoped<IConnectionEventRepository, EfConnectionEventRepository>();
builder.Services.AddScoped<IConnectionEventService, ConnectionEventService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();

// Swagger with API key
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "API Key needed to access POST endpoints",
        In = ParameterLocation.Header,
        Name = "X-Api-Key",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Dev exception page
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

// Middleware for API key
app.UseMiddleware<ApiKeyMiddleware>();

app.MapControllers();

app.Run();
