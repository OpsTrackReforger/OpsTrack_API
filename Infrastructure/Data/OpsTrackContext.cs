using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Pomelo.EntityFrameworkCore.MySql;

namespace Infrastructure.Data
{
    public class OpsTrackContext : DbContext
    {
        public DbSet<ConnectionEvent> ConnectionEvents => Set<ConnectionEvent>();
        public DbSet<Player> Players => Set<Player>();

        public OpsTrackContext(DbContextOptions<OpsTrackContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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


        }
    }
}
