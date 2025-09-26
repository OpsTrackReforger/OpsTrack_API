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
            //Player
            modelBuilder.Entity<Player>()
                .HasKey(p => p.GameIdentity);

            //ConnectionEvent
            modelBuilder.Entity<ConnectionEvent>()
                .HasKey(e => e.EventId);

            modelBuilder.Entity<ConnectionEvent>()
                .HasOne(e => e.Player)
                .WithMany(p => p.ConnectionEvents)
                .HasForeignKey(e => e.GameIdentity)
                .HasPrincipalKey(p => p.GameIdentity)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EventType>()
                .HasKey(et => et.eventTypeId);

            modelBuilder.Entity<Event>(builder =>
            {
                //Primary key
                builder.HasKey(e => e.EventId);

                //Required fields
                builder.Property(e => e.TimeStamp)
                    .IsRequired();

                //Relations to EventType
                builder.HasOne(e => e.EventType)
                       .WithMany()
                       .HasForeignKey(e => e.EventTypeId)
                       .OnDelete(DeleteBehavior.Restrict);
            });



        }
    }
}
