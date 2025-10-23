using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configurations
{
    public class CombatEventConfig : IEntityTypeConfiguration<CombatEvent>
    {
        public void Configure(EntityTypeBuilder<CombatEvent> builder)
        {
            // Primary key also foreign key to Event
            builder.HasKey(ce => ce.EventId);

            builder.HasOne(c => c.Event)
                .WithOne() // Event doesn't know CombatEvent
                .HasForeignKey<CombatEvent>(ce => ce.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // Actor (nullable)
            builder.HasOne(c => c.Actor)
                .WithMany(p => p.CombatEventsAsActor)
                .HasForeignKey(c => c.ActorId)
                .HasPrincipalKey(p => p.GameIdentity)
                .IsRequired(false) // ← Dette er vigtigt!
                .OnDelete(DeleteBehavior.Restrict);

            // Victim (nullable)
            builder.HasOne(c => c.Victim)
                .WithMany(p => p.CombatEventsAsVictim)
                .HasForeignKey(c => c.VictimId)
                .HasPrincipalKey(p => p.GameIdentity)
                .IsRequired(false) // ← Dette er vigtigt!
                .OnDelete(DeleteBehavior.Restrict);

            // Fields
            builder.Property(ce => ce.Weapon)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(ce => ce.Distance)
                .IsRequired();

            builder.Property(ce => ce.IsTeamKill)
                .IsRequired();

            builder.Property(ce => ce.ActorFaction)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(ce => ce.VictimFaction)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(ce => ce.ActorName)
                .HasMaxLength(100)
                .IsRequired(false);
            builder.Property(ce => ce.VictimName)
                .HasMaxLength(100)
                .IsRequired(false);
        }
    }

}
