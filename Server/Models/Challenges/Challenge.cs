using WebServer.Models.Auth;

namespace WebServer.Models.Challenges;

public class Challenge
{
    public required Guid Id { get; init; }
    public required string Title { get; set; }
    public required string Solution { get; set; }

    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public User? CreatedBy { get; set; }
    public Guid? CreatedById { get; set; }

    public List<TestCase> TestCases { get; set; } = [];
}
