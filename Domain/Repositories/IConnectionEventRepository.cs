using Domain.Entities;

namespace Domain.Repositories
{
    public interface IConnectionEventRepository
    {
        Task<ConnectionEvent?> GetByIdAsync(int eventId);
        Task<IEnumerable<ConnectionEvent>> GetByPlayerIdAsync(string gameIdentity);
        Task<IEnumerable<ConnectionEvent>> GetLatestAsync(int count);
        Task AddAsync(ConnectionEvent connectionEvent);
        Task SaveChangesAsync();
    }
}
