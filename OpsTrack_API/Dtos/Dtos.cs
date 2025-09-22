namespace OpsTrack_API.Dtos
{
    //Input
    public record PlayerEventRequest(string GameIdentity, string Name);

    //Output
    public record ConnectionEventResponse(
        int EventId,
        string GameIdentity,
        string Name,
        string EventType,
        DateTime Timestamp
    );

    public record PlayerResponse(
        string GameIdentity,
        string LastKnownName,
        DateTime FirstSeen,
        DateTime LastSeen
    );

}
