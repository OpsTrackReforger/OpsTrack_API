using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class EventType
    {
        public int eventTypeId { get; set; } 
        public string name { get; set; } = null!;
        public string category { get; set; } = null!;
        public string description { get; set; }

    }
}
