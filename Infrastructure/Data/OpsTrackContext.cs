using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Pomelo.EntityFrameworkCore.MySql;
using System;

namespace Infrastructure.Data
{
    public class OpsTrackContext : DbContext
    {
        public DbSet<ConnectionEvent> ConnectionEvent => Set<ConnectionEvent>();
        public DbSet<Player> Player => Set<Player>();
        public DbSet<EventType> EventType => Set<EventType>();
        public DbSet<Event> Event => Set<Event>();
        public DbSet<CombatEvent> CombatEvent => Set<CombatEvent>();


        public OpsTrackContext(DbContextOptions<OpsTrackContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OpsTrackContext).Assembly);
        }
    }
}
