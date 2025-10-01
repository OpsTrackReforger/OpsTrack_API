using Application.Dtos;
using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CombatEventService : ICombatEventService
    {
        private readonly ICombatEventRepository _combatRepo;
        private readonly IEventRepository _eventRepo;

        public CombatEventService(ICombatEventRepository combatRepo, IEventRepository eventRepo)
        {
            _combatRepo = combatRepo;
            _eventRepo = eventRepo;
        }

        public async Task<CombatEventResponse?> GetByIdAsync(int eventId)
        {
            var e = await _combatRepo.GetByIdAsync(eventId);
            if (e == null) return null;

            return new CombatEventResponse
            (
                EventId: e.EventId,
                ActorId: e.ActorId,
                ActorName: e.Actor?.LastKnownName ?? "Unknown",
                VictimId: e.VictimId,
                VictimName: e.Victim?.LastKnownName ?? "Unknown",
                Weapon: e.Weapon,
                Distance: e.Distance,
                IsTeamKill: e.IsTeamKill,
                TimeStamp: e.Event.TimeStamp,
                EventTypeName: e.Event.EventType?.name ?? "Unknown"
            );
        }


        public async Task<IEnumerable<CombatEventResponse>> GetAllAsync()
        {
            var events = await _combatRepo.GetAllAsync();

            return events.Select(e => new CombatEventResponse(
                EventId: e.EventId,
                ActorId: e.ActorId,
                ActorName: e.Actor?.LastKnownName ?? "Unknown",
                VictimId: e.VictimId,
                VictimName: e.Victim?.LastKnownName ?? "Unknown",
                Weapon: e.Weapon,
                Distance: e.Distance,
                IsTeamKill: e.IsTeamKill,
                TimeStamp: e.Event.TimeStamp,
                EventTypeName: e.Event.EventType?.name ?? "Unknown"
            ));
        }

        public async Task<CombatEventResponse> RegisterCombatEventAsync(CombatEventRequest req)
        {
            var timestamp = DateTime.UtcNow;

            var ev = new Event
            {
                TimeStamp = timestamp,
                EventTypeId = req.EventTypeId
            };

            await _eventRepo.AddAsync(ev);
            await _eventRepo.SaveChangesAsync();

            var combat = new CombatEvent
            {
                Event = ev,
                ActorId = req.ActorId,
                VictimId = req.VictimId,
                Weapon = req.Weapon,
                Distance = req.Distance,
                IsTeamKill = req.IsTeamKill
            };

            await _combatRepo.AddAsync(combat);
            await _combatRepo.SaveChangesAsync();

            // Sørg for at EventType er inkluderet
            ev.EventType = await _eventRepo.GetEventTypeByIdAsync(ev.EventTypeId);

            return new CombatEventResponse(
                EventId: combat.EventId,
                ActorId: combat.ActorId,
                ActorName: combat.Actor?.LastKnownName ?? "Unknown",
                VictimId: combat.VictimId,
                VictimName: combat.Victim?.LastKnownName ?? "Unknown",
                Weapon: combat.Weapon,
                Distance: combat.Distance,
                IsTeamKill: combat.IsTeamKill,
                TimeStamp: combat.Event.TimeStamp,
                EventTypeName: combat.Event.EventType?.name ?? "Unknown"
            );
        }

    }

}
