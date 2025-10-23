using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helpers
{
    public static class PlayerHelpers
    {
        public static bool IsValidPlayerId(string playerId)
        {
            if (string.IsNullOrWhiteSpace(playerId))
            {
                return false;
            }

            if (playerId.Equals("Environment", StringComparison.OrdinalIgnoreCase)) return false;
            if (playerId.Equals("AI", StringComparison.OrdinalIgnoreCase)) return false;
            

            return true;
            // Additional validation logic can be added here if needed
        }
    }
}
