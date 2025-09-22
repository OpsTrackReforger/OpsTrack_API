using Application.Dtos;
using Application.Enums;
using Domain.Entities;
using Domain.Repositories;
namespace Application.Services
{
    public class ConnectionEventService : IConnectionEventService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IConnectionEventRepository _eventRepository;

        public ConnectionEventService(IPlayerRepository playerRepository, IConnectionEventRepository eventRepository)
        {
            _playerRepository = playerRepository;
            _eventRepository = eventRepository;
        }

        public async Task<ConnectionEventResponse> RegisterConnectionEventAsync(string gameIdentity, string name, ConnectionEventType type)
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

            // Opret event
            var ev = new ConnectionEvent
            {
                GameIdentity = gameIdentity,
                Name = name,
                EventType = type.ToString().ToLower(), // gemmes som string i DB
                Timestamp = timestamp,
                Player = player
            };

            await _eventRepository.AddAsync(ev);

            // Commit alle ændringer samlet
            await _playerRepository.SaveChangesAsync();
            await _eventRepository.SaveChangesAsync();

            return new ConnectionEventResponse(
                ev.EventId,
                ev.GameIdentity,
                ev.Name,
                ev.EventType,
                ev.Timestamp
            );
        }
    }
}