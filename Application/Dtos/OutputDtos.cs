using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public record ConnectionEventResponse(
        int EventId,
        string GameIdentity,
        string Name,
        string EventType,
        DateTime Timestamp
    );

    public record PlayerResponse(
        string GameIdentity,
        string LastKnownName,
        DateTime FirstSeen,
        DateTime LastSeen
    );

    public record CombatEventResponse(
        int EventId,
        string? ActorId,
        string? ActorName,
        string? ActorFaction,
        string? VictimId,
        string? VictimName,
        string? VictimFaction,
        string? Weapon,
        int Distance,
        bool IsTeamKill,
        DateTime TimeStamp,
        string EventTypeName
    );
}
