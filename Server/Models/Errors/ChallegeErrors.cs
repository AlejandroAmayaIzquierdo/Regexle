namespace WebServer.Models.Errors;

public class ChallengeErrors
{
    public static readonly Error ChallengeNotFound = new(
        "Challenge.NotFound",
        "No challenge is scheduled for today."
    );

    public static readonly Error RegexDoesNotPassAllTestCases = new(
        "Challenge.RegexDoesNotPassAllTestCases",
        "The provided regex does not pass all test cases."
    );

    public static readonly Error InvalidRegex = new(
        "Challenge.InvalidRegex",
        "The provided regex is invalid."
    );
}
