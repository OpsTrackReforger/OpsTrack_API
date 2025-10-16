using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EfCombatEventRepository : ICombatEventRepository
    {
        private readonly OpsTrackContext _context;

        public EfCombatEventRepository(OpsTrackContext context)
        {
            _context = context;
        }

        public async Task<CombatEvent?> GetByIdAsync(int eventId) =>
            await _context.CombatEvent
                .Include(c => c.Event)
                    .ThenInclude(e => e.EventType)
                .Include(c => c.Actor)
                .Include(c => c.Victim)
                .FirstOrDefaultAsync(c => c.EventId == eventId);

        public async Task<IEnumerable<CombatEvent>> GetAllAsync() =>
            await _context.CombatEvent
                .Include(c => c.Event)
                    .ThenInclude(e => e.EventType)
                .Include(c => c.Actor)
                .Include(c => c.Victim)
                .OrderByDescending(c => c.Event.TimeStamp)
                .AsNoTracking()
                .ToListAsync();

        public async Task AddAsync(CombatEvent combatEvent) =>
            await _context.CombatEvent.AddAsync(combatEvent);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();

        public async Task<IEnumerable<CombatEvent>> GetByDateRangeAsync(DateTime start, DateTime end)
        {

            return await _context.CombatEvent
                .Include(c => c.Event)
                    .ThenInclude(e => e.EventType)
                .Include(c => c.Actor)
                .Include(c => c.Victim)
                .Where(c => c.Event.TimeStamp >= start && c.Event.TimeStamp <= end)
                .OrderByDescending(c => c.Event.TimeStamp)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
