using Application.Dtos;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IConnectionEventRepository _eventRepository;

        public PlayerService(IPlayerRepository playerRepository, IConnectionEventRepository connectionEventRepository)
        {
            _eventRepository = connectionEventRepository;
            _playerRepository = playerRepository;
        }

        public async Task<IEnumerable<PlayerResponse>> GetAllAsync()
        {
            var players = await _playerRepository.GetAllAsync();

            return players.Select(p => new PlayerResponse(
                p.LastKnownName,
                p.GameIdentity,
                p.FirstSeen,
                p.LastSeen
                ));
        }

        public async Task<IEnumerable<PlayerResponse>> GetOnline()
        {
            var latestEvents = await _eventRepository.GetLAtestEventsByPlayerAsync();

            var onlinePlayers = latestEvents
                .Where(e => e != null && e.EventType == "join") // kun spillere der sidst joinede
                .Select(e => new PlayerResponse(
                    e.GameIdentity,
                    e.Name,
                    e.Timestamp, // evt. FirstSeen kan hentes fra Player
                    e.Timestamp  // LastSeen, kan også hentes fra Player
                ));

            return onlinePlayers;
        }
    }
}

