namespace WebServer.Models.Challenges;

public class Challenge
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
