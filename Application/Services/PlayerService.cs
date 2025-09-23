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
    }
}

