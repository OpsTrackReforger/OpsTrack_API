using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _repo;

        public EventService(IEventRepository repo)
        {
            _repo = repo;
        }

        public async Task<Event?> GetByIdAsync(int eventId) =>
            await _repo.GetByIdAsync(eventId);

        public async Task<IEnumerable<Event>> GetAllAsync() =>
            await _repo.GetAllAsync();

        public async Task<IEnumerable<Event>> GetByTypeAsync(int eventTypeId) =>
            await _repo.GetByTypeAsync(eventTypeId);

        public async Task<Event> RegisterEventAsync(Event newEvent)
        {
            await _repo.AddAsync(newEvent);
            await _repo.SaveChangesAsync();
            return newEvent;
        }
    }

}
