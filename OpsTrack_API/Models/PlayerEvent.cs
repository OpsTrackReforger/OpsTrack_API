

namespace OpsTrack_API.Models
{
    public class PlayerEvent
    {
        public int Id { get; set; }          // Primary key
        public int PlayerId { get; set; }    // Player ID from server (SteamID)
        public string Name { get; set; } = ""; // Player name at the time of the event
        public string EventType { get; set; } = ""; // "join" or "leave"
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    }
}
