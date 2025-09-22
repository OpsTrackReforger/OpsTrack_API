using Application.Dtos;
using Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IConnectionEventService
    {
        Task<ConnectionEventResponse> RegisterConnectionEventAsync(string gameIdentity, string name, ConnectionEventType type);

    }
}
