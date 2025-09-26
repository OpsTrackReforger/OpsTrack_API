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
            //primary key also foreign key to Event
            builder.HasKey(ce => ce.EventId);

            builder.HasOne(c => c.Event)
                .WithOne() //Event doesnt know CombatEvent
                .HasForeignKey<CombatEvent>(ce => ce.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            //Relations to Actor (nullable)
            builder.HasOne(ce => ce.Actor)
                .WithMany() //Player doesnt know CombatEvents
                .HasForeignKey(ce => ce.ActorId)
                .OnDelete(DeleteBehavior.SetNull);

            //Relations to Victim (nullable)
            builder.HasOne(ce => ce.Victim)
                .WithMany() //Player doesnt know CombatEvents
                .HasForeignKey(ce => ce.VictimId)
                .OnDelete(DeleteBehavior.SetNull);

            //Fields
            builder.Property(ce => ce.Weapon)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(ce => ce.Distance)
                .IsRequired();

            builder.Property(ce => ce.IsTeamKill)
                .IsRequired();
        }
    }
}
