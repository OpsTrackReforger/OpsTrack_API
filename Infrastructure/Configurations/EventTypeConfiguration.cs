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
    public class EventTypeConfiguration : IEntityTypeConfiguration<EventType>
    {
        public void Configure(EntityTypeBuilder<EventType> builder)
        {
            //Event type can be something like Join, Leave, Kill etc.

            //Primary key
            builder.HasKey(et => et.eventTypeId);
            //Required fields
            builder.Property(et => et.name)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(et => et.description)
                .HasMaxLength(500);

        }
    }
}
