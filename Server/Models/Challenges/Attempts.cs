using WebServer.Models.Auth;

namespace WebServer.Models.Challenges;

public class Attempts
{
    public int Id { get; set; }
    public int ChallengeId { get; set; }
    public Challenge? Challenge { get; set; } = null!;
    public int UserId { get; set; }
    public User? User { get; set; } = null!;
    public int AttemptCount { get; set; }
    public DateTime LastAttemptAt { get; set; }
}
