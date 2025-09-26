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
                GameIdentity: p.GameIdentity,
                LastKnownName: p.LastKnownName,
                FirstSeen: p.FirstSeen,
                LastSeen: p.LastSeen
                ));
        }

        public async Task<IEnumerable<PlayerResponse>> GetOnline()
        {
            var latestEvents = await _eventRepository.GetLatestEventsByPlayerAsync();

            var onlinePlayers = latestEvents
                .Where(e => e != null && e.Event.EventType.name == "join") // kun spillere der sidst joinede
                .Select(e => new PlayerResponse(
                    e.GameIdentity,
                    e.Name,
                    e.Event.TimeStamp, // evt. FirstSeen kan hentes fra Player
                    e.Event.TimeStamp  // LastSeen, kan også hentes fra Player
                ));

            return onlinePlayers;
        }
    }
}

