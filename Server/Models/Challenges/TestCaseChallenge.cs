using System.Text.Json.Serialization;

namespace WebServer.Models.Challenges;

public class TestCase
{
    public required Guid Id { get; init; }
    public required string Text { get; init; }
    public bool IsMatch { get; init; } = true;

    [JsonIgnore]
    public Challenge? Challenge { get; set; }
    public required Guid ChallengeId { get; set; }
}
