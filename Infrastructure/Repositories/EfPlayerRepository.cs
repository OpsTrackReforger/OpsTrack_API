
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EfPlayerRepository : IPlayerRepository
    {
        private readonly OpsTrackContext _context;

        public EfPlayerRepository(OpsTrackContext context)
        {
            _context = context;
        }

        public async Task<Player?> GetByIdAsync(string gameIdentity) =>
            await _context.Players.FindAsync(gameIdentity);

        public async Task<IEnumerable<Player>> GetAllAsync() =>
            await _context.Players.AsNoTracking().ToListAsync();

        public async Task AddAsync(Player player) =>
            await _context.Players.AddAsync(player);

        public Task UpdateAsync(Player player)
        {
            _context.Players.Update(player);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}