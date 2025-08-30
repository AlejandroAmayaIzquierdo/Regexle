namespace WebServer.Models.Challenges;

public class ChallengeSchedule
{
    public required Guid Id { get; init; }
    public required DateTime Date { get; init; }
    public required Guid ChallengeId { get; init; }
    public Challenge? Challenge { get; set; }
}
