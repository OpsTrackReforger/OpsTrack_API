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
            await _context.ConnectionEvents.FindAsync(eventId);

        public async Task<IEnumerable<ConnectionEvent>> GetByPlayerIdAsync(string gameIdentity) =>
            await _context.ConnectionEvents
                .Where(e => e.GameIdentity == gameIdentity)
                .AsNoTracking()
                .ToListAsync();

        public async Task<IEnumerable<ConnectionEvent>> GetLatestAsync(int count) =>
            await _context.ConnectionEvents
                .OrderByDescending(e => e.Timestamp)
                .Take(count)
                .AsNoTracking()
                .ToListAsync();

        public async Task<IEnumerable<ConnectionEvent>> GetAllAsync() =>
            await _context.ConnectionEvents
                .OrderByDescending(e => e.Timestamp)
                .AsNoTracking()
                .ToListAsync();

        public async Task AddAsync(ConnectionEvent connectionEvent) =>
            await _context.ConnectionEvents.AddAsync(connectionEvent);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();

    }
}