using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Pomelo.EntityFrameworkCore.MySql;
using System;

namespace Infrastructure.Data
{
    public class OpsTrackContext : DbContext
    {
        public DbSet<ConnectionEvent> ConnectionEvents => Set<ConnectionEvent>();
        public DbSet<Player> Players => Set<Player>();
        public DbSet<EventType> EventTypes => Set<EventType>();
        public DbSet<Event> Events => Set<Event>();

        public OpsTrackContext(DbContextOptions<OpsTrackContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OpsTrackContext).Assembly);
        }
    }
}
