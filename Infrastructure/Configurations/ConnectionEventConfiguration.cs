using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ConnectionEventConfiguration : IEntityTypeConfiguration<ConnectionEvent>
    {
        public void Configure(EntityTypeBuilder<ConnectionEvent> builder)
        {
            //Primary key also foreign key to Event
            builder.HasKey(ce => ce.EventId);

            builder.HasOne(c => c.Event)
                .WithOne() //Event doesnt know ConnectionEvent
                .HasForeignKey<ConnectionEvent>(ce => ce.EventId)
                .OnDelete(DeleteBehavior.Cascade);
            
            //Relations to Player
            builder.HasOne(ce => ce.Player)
                .WithMany(p => p.ConnectionEvents) //Player knows ConnectionEvents
                .HasForeignKey(ce => ce.GameIdentity)
                .HasPrincipalKey(p => p.GameIdentity)
                .OnDelete(DeleteBehavior.Cascade);

            //Fields
            builder.Property(ce => ce.Name)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
