namespace Domain.Entities
{
    public class ConnectionEvent
    {
        public int EventId { get; set; }          // Primary key auto-incremented
        public string GameIdentity { get; set; } = null!;   // Foreign Key Game Identity ID of the player
        public string Name { get; set; } = null!; // Player name at the time of the event
        public string EventType { get; set; } = null!; // "join" or "leave"
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // Foreign key to Player
        public Player Player { get; set; } = null!;
    }
}
