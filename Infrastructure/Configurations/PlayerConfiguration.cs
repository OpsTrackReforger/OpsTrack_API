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
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            //Primary key
            builder.HasKey(p => p.GameIdentity);

            //Required fields
            builder.Property(p => p.GameIdentity)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(p => p.LastKnownName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.FirstSeen)
               .IsRequired();

            builder.Property(p => p.LastSeen)
                   .IsRequired();

            //Relations to ConnectionEvents
            builder.HasMany(p => p.ConnectionEvents)
                .WithOne(ce => ce.Player) //ConnectionEvent knows Player
                .HasForeignKey(ce => ce.GameIdentity)
                .HasPrincipalKey(p => p.GameIdentity)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
