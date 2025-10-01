using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EfConnectionEventRepository : IConnectionEventRepository
    {
        private readonly OpsTrackContext _context;

        public EfConnectionEventRepository(OpsTrackContext context)
        {
            _context = context;
        }

        public async Task<ConnectionEvent?> GetByIdAsync(int eventId) =>
            await _context.ConnectionEvent
                .Include(e => e.Event)
                    .ThenInclude(ev => ev.EventType)
                .Include(e => e.Player)
                .FirstOrDefaultAsync(e => e.EventId == eventId);

        public async Task<IEnumerable<ConnectionEvent>> GetByPlayerGameIdentityAsync(string gameIdentity) =>
            await _context.ConnectionEvent
                .Include(e => e.Event)
                    .ThenInclude(ev => ev.EventType)
                .Where(e => e.GameIdentity == gameIdentity)
                .AsNoTracking()
                .ToListAsync();

        public async Task<IEnumerable<ConnectionEvent>> GetLatestAsync(int count) =>
            await _context.ConnectionEvent
                .Include(e => e.Event)
                    .ThenInclude(ev => ev.EventType)
                .OrderByDescending(e => e.Event.TimeStamp)
                .Take(count)
                .AsNoTracking()
                .ToListAsync();

        public async Task<IEnumerable<ConnectionEvent>> GetLatestEventsByPlayerAsync()
        {
            var all = await _context.ConnectionEvent
                .Include(e => e.Event)
                    .ThenInclude(ev => ev.EventType)
                .AsNoTracking()
                .ToListAsync();

            return all
                .GroupBy(e => e.GameIdentity)
                .Select(g => g.OrderByDescending(e => e.Event.TimeStamp).FirstOrDefault()!)
                .ToList();
        }

        public async Task<IEnumerable<ConnectionEvent>> GetAllAsync() =>
            await _context.ConnectionEvent
                .Include(e => e.Event)
                    .ThenInclude(ev => ev.EventType)
                .OrderByDescending(e => e.Event.TimeStamp)
                .AsNoTracking()
                .ToListAsync();

        public async Task AddAsync(ConnectionEvent connectionEvent) =>
            await _context.ConnectionEvent.AddAsync(connectionEvent);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
