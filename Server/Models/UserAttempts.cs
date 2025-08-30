namespace WebServer.Models;

public class UserAttempt
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required Guid ChallengeId { get; init; }
    public required DateTime AttemptedAt { get; init; }
    public required bool IsCorrect { get; init; }
    public required string RegexSubmitted { get; init; }
}
