using Domain.Entities;

namespace Domain.Repositories
{
    public interface IConnectionEventRepository
    {
        Task<ConnectionEvent?> GetByIdAsync(int eventId);
        Task<IEnumerable<ConnectionEvent>> GetByPlayerGameIdentityAsync(string gameIdentity);
        Task<IEnumerable<ConnectionEvent>> GetLatestAsync(int count);
        Task AddAsync(ConnectionEvent connectionEvent);
        Task<IEnumerable<ConnectionEvent>> GetLatestEventsByPlayerAsync();
        Task<IEnumerable<ConnectionEvent>> GetAllAsync();
        Task SaveChangesAsync();
    }
}
