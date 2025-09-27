using Application.Dtos;
using Application.Enums;
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
                .Where(e => e != null && e.Event.EventTypeId != (int)ConnectionEventType.LEAVE)
                .Select(e => new PlayerResponse(
                    e.GameIdentity,
                    e.Name,
                    e.Player?.FirstSeen ?? e.Event.TimeStamp,
                    e.Player?.LastSeen ?? e.Event.TimeStamp
                ));

            return onlinePlayers;
        }
    }
}

