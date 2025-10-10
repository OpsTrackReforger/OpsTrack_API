using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IEventRepository
    {
        Task<Event?> GetByIdAsync(int eventId);
        Task<IEnumerable<Event>> GetAllAsync();
        Task<IEnumerable<Event>> GetByTypeAsync(int eventTypeId);
        Task<EventType?> GetEventTypeByIdAsync(int eventTypeId);
        Task AddAsync(Event ev);
        Task SaveChangesAsync();
    }
}
