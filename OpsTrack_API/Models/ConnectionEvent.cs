

namespace OpsTrack_API.Models
{
    public class ConnectionEvent
    {
        public int Id { get; set; }          // Primary key
        public string GameIdentity { get; set; }    // Game Identity ID of the player
        public string Name { get; set; } = ""; // Player name at the time of the event
        public string EventType { get; set; } = ""; // "join" or "leave"
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    }
}
