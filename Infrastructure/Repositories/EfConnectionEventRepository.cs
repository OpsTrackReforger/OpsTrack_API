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
            await _context.ConnectionEvents
                .Include(e => e.Event)
                .Include(e => e.Player)
                .FirstOrDefaultAsync(e => e.EventId == eventId);

        public async Task<IEnumerable<ConnectionEvent>> GetByPlayerIdAsync(string gameIdentity) =>
            await _context.ConnectionEvents
                .Include(e => e.Event)
                .Where(e => e.GameIdentity == gameIdentity)
                .AsNoTracking()
                .ToListAsync();

        public async Task<IEnumerable<ConnectionEvent>> GetLatestAsync(int count) =>
            await _context.ConnectionEvents
                .Include(e => e.Event)
                .OrderByDescending(e => e.Event.TimeStamp)
                .Take(count)
                .AsNoTracking()
                .ToListAsync();

        public async Task<IEnumerable<ConnectionEvent>> GetLatestEventsByPlayerAsync() =>
            await _context.ConnectionEvents
                .Include(e => e.Event)
                .GroupBy(e => e.GameIdentity)
                .Select(g => g.OrderByDescending(e => e.Event.TimeStamp).FirstOrDefault()!)
                .ToListAsync();

        public async Task<IEnumerable<ConnectionEvent>> GetAllAsync() =>
            await _context.ConnectionEvents
                .Include(e => e.Event)
                .OrderByDescending(e => e.Event.TimeStamp)
                .AsNoTracking()
                .ToListAsync();

        public async Task AddAsync(ConnectionEvent connectionEvent) =>
            await _context.ConnectionEvents.AddAsync(connectionEvent);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
