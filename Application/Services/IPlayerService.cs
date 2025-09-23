using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IPlayerService
    {
        Task<IEnumerable<PlayerResponse>> GetAllAsync();
        Task<IEnumerable<PlayerResponse>> GetOnline();
    }
}
