using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public record PlayerEventRequest(string GameIdentity, string Name);
    public record CombatEventRequest(
        int EventTypeId,
        string ActorId,
        string? ActorFaction,
        string VictimId,
        string? VictimFaction,
        string? Weapon,
        int Distance,
        bool IsTeamKill
    );

}
