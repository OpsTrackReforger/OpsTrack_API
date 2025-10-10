using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Event
    {
        public int EventId { get; set; } // Primary key
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public int EventTypeId { get; set; } // Foreign key to EventType


        //navigation property for related event type
        public EventType EventType { get; set; } = null!;
    }
}
