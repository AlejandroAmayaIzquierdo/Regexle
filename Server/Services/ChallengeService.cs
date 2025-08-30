using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using WebServer.Data;
using WebServer.Models;
using WebServer.Models.Challenges;
using WebServer.Models.Errors;

namespace WebServer.Services;

public class ChallengeService(ApplicationDbContext dbContext)
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Challenge?> GetTodayChallengeAsync()
    {
        // For early develop return any challenge

        var challenge = await _dbContext.Challenges.Include(c => c.TestCases).FirstOrDefaultAsync();

        return challenge;

        /*
        var challenge = await _dbContext
            .ChallengeSchedules.Include(cs => cs.Challenge)
            .FirstOrDefaultAsync(cs => cs.Date == DateTime.UtcNow.Date);

        if (challenge is null || challenge.Challenge is null)
            return Result<Challenge>.Failure(ChallengeErrors.ChallengeNotFound);

        return Result<Challenge>.Success(challenge.Challenge);
        */
    }

    public async Task<Result> CheckRegex(string userRegex, Challenge? challenge = null)
    {
        if (string.IsNullOrWhiteSpace(userRegex))
            return Result.Failure(ChallengeErrors.InvalidRegex);

        challenge ??= await GetTodayChallengeAsync();

        if (challenge is null)
            return Result.Failure(ChallengeErrors.ChallengeNotFound);

        var testCases = challenge.TestCases;

        var regex = new Regex(userRegex);

        foreach (var testCase in testCases)
        {
            try
            {
                var isMatch = regex.IsMatch(testCase.Text);

                // If the test case is marked as a match, but the regex does not match, fail
                if (testCase.IsMatch && !isMatch)
                    return Result.Failure(ChallengeErrors.RegexDoesNotPassAllTestCases);
            }
            catch (Exception)
            {
                return Result.Failure(ChallengeErrors.InvalidRegex);
            }
        }

        return Result.Success();
    }
}
