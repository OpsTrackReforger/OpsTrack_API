using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CombatEvent
    {
        public int EventId { get; set; } // Primary key/Foreign key to Event
        public string ActorId{ get; set; } // Foreign key to Player (Attacker) null if ai/environment
        public string VictimId { get; set; }   // Foreign key to Player (Victim) null if ai/environment
        public string? Weapon { get; set; } // Weapon used in the event, if applicable
        public int Distance { get; set; } // Distance between attacker and victim, if applicable
        public bool IsTeamKill { get; set; } // Indicates if the event was a team kill


        // Navigation properties
        public Player Actor { get; set; } //Can be null if ai/environment
        public Player Victim { get; set; } //Can be null if ai/environment
        public Event Event { get; set; } = null!;
    }
}
