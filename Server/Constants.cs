using WebServer.DTOs.Auth;

namespace WebServer;

public class Constants
{
    public const int MaxDailyAttempts = 3;

    public static UserDto GuessUser =>
        new()
        {
            UserName = Guid.NewGuid().ToString(),
            Email = $"{Guid.NewGuid()}@guess.com",
            Password = "guess123",
        };
}
