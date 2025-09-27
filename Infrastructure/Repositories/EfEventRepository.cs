using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EfEventRepository : IEventRepository
    {
        private readonly OpsTrackContext _context;

        public EfEventRepository(OpsTrackContext context)
        {
            _context = context;
        }

        public async Task<Event?> GetByIdAsync(int eventId) =>
            await _context.Event
                .Include(e => e.EventType)
                .FirstOrDefaultAsync(e => e.EventId == eventId);

        public async Task<IEnumerable<Event>> GetAllAsync() =>
            await _context.Event
                .Include(e => e.EventType)
                .OrderByDescending(e => e.TimeStamp)
                .AsNoTracking()
                .ToListAsync();

        public async Task<IEnumerable<Event>> GetByTypeAsync(int eventTypeId) =>
            await _context.Event
                .Include(e => e.EventType)
                .Where(e => e.EventTypeId == eventTypeId)
                .OrderByDescending(e => e.TimeStamp)
                .AsNoTracking()
                .ToListAsync();

        public async Task AddAsync(Event ev) =>
            await _context.Event.AddAsync(ev);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();

    }
}
