namespace Domain.Entities
{
    public class ConnectionEvent
    {
        public int EventId { get; set; }          // Primary key auto-incremented
        public string GameIdentity { get; set; } = null!;   // Foreign Key Game Identity ID of the player
        public string Name { get; set; } = null!; // Player name at the time of the event

        // Foreign key to Player
        public Player Player { get; set; } = null!;
        public Event Event { get; set; } = null!;
    }
}
