using WebServer.Models.Challenges;

namespace WebServer.Endpoints;

public class ChallengeHandlers
{
    public static async Task<IResult> CreateAsync()
    {
        await Task.CompletedTask;
        var challenge = new Challenge
        {
            Id = Guid.NewGuid(),
            Title = "Test",
            Description = "Test Description",
            CreatedAt = DateTime.UtcNow,
        };
        return Results.Created($"/challenges/{challenge.Id}", challenge);
    }
}
