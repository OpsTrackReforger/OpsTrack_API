using Application.Dtos;
using Application.Enums;
using Domain.Entities;
using Domain.Repositories;
using System.Linq;

namespace Application.Services
{
    public class ConnectionEventService : IConnectionEventService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IConnectionEventRepository _connectionEventRepository;

        public ConnectionEventService(IPlayerRepository playerRepository, IEventRepository eventRepository, IConnectionEventRepository connectionEventRepository)
        {
            _playerRepository = playerRepository;
            _eventRepository = eventRepository;
            _connectionEventRepository = connectionEventRepository;
        }

        public async Task<ConnectionEventResponse> RegisterConnectionEventAsync(
            string gameIdentity,
            string name,
            ConnectionEventType type)
        {
            var timestamp = DateTime.UtcNow;

            // Find eller opret spiller
            var player = await _playerRepository.GetByIdAsync(gameIdentity);
            if (player == null)
            {
                player = new Player
                {
                    GameIdentity = gameIdentity,
                    LastKnownName = name,
                    FirstSeen = timestamp,
                    LastSeen = timestamp
                };
                await _playerRepository.AddAsync(player);
            }
            else
            {
                player.LastKnownName = name;
                player.LastSeen = timestamp;
                await _playerRepository.UpdateAsync(player);
            }

            // Opret Event (Join/Leave)
            var ev = new Event
            {
                TimeStamp = timestamp,
                EventTypeId = (type == ConnectionEventType.JOIN ? 3 : 4) // fx 3=Join, 4=Leave
            };
            await _eventRepository.AddAsync(ev);

            // Opret ConnectionEvent
            var connEvent = new ConnectionEvent
            {
                Event = ev,
                GameIdentity = gameIdentity,
                Name = name,
                Player = player
            };
            await _connectionEventRepository.AddAsync(connEvent);

            // Commit
            await _playerRepository.SaveChangesAsync();
            await _eventRepository.SaveChangesAsync();
            await _connectionEventRepository.SaveChangesAsync();

            return new ConnectionEventResponse(
                connEvent.EventId,
                connEvent.GameIdentity,
                connEvent.Name,
                ev.EventType.name,   // eller slå op i EventType for at returnere navnet
                ev.TimeStamp
            );
        }

        public async Task<IEnumerable<ConnectionEventResponse>> GetAllAsync()
        {
            var events = await _connectionEventRepository.GetAllAsync();

            return events.Select(e => new ConnectionEventResponse(
                e.EventId,
                e.GameIdentity,
                e.Name,
                e.Event.EventType.name,   // eller e.Event.EventType.Name
                e.Event.TimeStamp
            ));
        }
    }
}
