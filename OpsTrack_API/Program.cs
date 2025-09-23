using Application.Services;
using Domain.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
else // default til Sqlite
{
    //SqLite datapath is always in solution folder /Data
    var solutionRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\.."));
    var dataPath = Path.Combine(solutionRoot, "Data");
    Directory.CreateDirectory(dataPath);
    var connectionString = $"Data Source={Path.Combine(dataPath, "opstrack.db")}";

    builder.Services.AddDbContext<OpsTrackContext>(options =>
        options.UseSqlite(connectionString));
}

//Add repositories and services
builder.Services.AddScoped<IPlayerRepository, EfPlayerRepository>();
builder.Services.AddScoped<IConnectionEventRepository, EfConnectionEventRepository>();
builder.Services.AddScoped<IConnectionEventService, ConnectionEventService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
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