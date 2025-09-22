using Domain.Entities;

namespace Domain.Repositories
{
    public interface IPlayerRepository
    {
        Task<Player?> GetByIdAsync(string gameIdentity);
        Task<IEnumerable<Player>> GetAllAsync();
        Task AddAsync(Player player);
        Task UpdateAsync(Player player);
        Task SaveChangesAsync();
    }
}