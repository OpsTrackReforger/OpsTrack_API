namespace Domain.Entities
{
    public class Player
    {
        public string GameIdentity { get; set; } = null!;    // Game Identity ID of the player
        public string LastKnownName { get; set; } = "";// Player name at the time of the event
        public DateTime FirstSeen { get; set; }
        public DateTime LastSeen { get; set; }


        //Navigation propertyfor related connection events
        public ICollection<ConnectionEvent> ConnectionEvents { get; set; } = new List<ConnectionEvent>();

        // Navigation til combat events hvor spilleren var actor
        public ICollection<CombatEvent> CombatEventsAsActor { get; set; } = new List<CombatEvent>();

        // Navigation til combat events hvor spilleren var victim
        public ICollection<CombatEvent> CombatEventsAsVictim { get; set; } = new List<CombatEvent>();
    }
}
