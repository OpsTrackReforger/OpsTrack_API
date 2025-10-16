using Application.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface ICombatEventService
    {
        Task<CombatEventResponse?> GetByIdAsync(int eventId);
        Task<IEnumerable<CombatEventResponse>> GetAllAsync();
        Task<CombatEventResponse> RegisterCombatEventAsync(CombatEventRequest req);
        Task<IEnumerable<CombatEventResponse>> GetByDateRangeAsync(DateTime start, DateTime end);
    }

}
