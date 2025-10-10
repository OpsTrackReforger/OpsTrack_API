using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ICombatEventRepository
    {
        Task<CombatEvent?> GetByIdAsync(int eventId);
        Task<IEnumerable<CombatEvent>> GetAllAsync();
        Task AddAsync(CombatEvent combatEvent);
        Task SaveChangesAsync();
    }
}
