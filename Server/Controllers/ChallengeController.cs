using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServer.DTOs.Challenges;
using WebServer.Models.Challenges;
using WebServer.Models.Errors;
using WebServer.Services;

namespace WebServer.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = "UserAccess")]
public class ChallengesController(ChallengeService challengeService) : ControllerBase
{
    private readonly ChallengeService _challengeService = challengeService;

    [HttpGet("today")]
    public async Task<IResult> GetTodayChallenge()
    {
        var challenge = await _challengeService.GetTodayChallengeAsync();

        if (challenge is null)
            return Results.Problem("Challenge not found", statusCode: 404);

        return Results.Ok(challenge);
    }

    public record SubmitRequest(string Answer);

    [HttpPost("submit")]
    public async Task<IResult> SubmitChallenge(
        SubmitRequest request,
        [FromServices] UserContext userContext
    )
    {
        if (Regex.Escape(request.Answer) == request.Answer)
            return Results.Problem("This is not a valid regex", statusCode: 400);

        var challenge = await _challengeService.GetTodayChallengeAsync();
        var result = await _challengeService.CheckRegex(request.Answer, challenge);

        var user = await userContext.GetUser(true);

        if (user is null)
            return Results.Problem("User not found", statusCode: 404);

        //new Attempt

        if (result.IsSuccess)
            return Results.Ok(
                new SubmitResponseDto
                {
                    IsSuccess = true,
                    Answer = request.Answer,
                    AttemptsLeft = 0,
                }
            );

        int statusCode = 200;
        if (result.Error == ChallengeErrors.ChallengeNotFound)
            statusCode = 404;
        else if (result.Error == ChallengeErrors.InvalidRegex)
            statusCode = 400;
        // else if (result.Error == ChallengeErrors.RegexDoesNotPassAllTestCases)
        //     statusCode = 422;

        if (statusCode > 299)
            return Results.Problem(result.Error!.Description, statusCode: statusCode);

        var response = new SubmitResponseDto
        {
            IsSuccess = false,
            ErrorMessage = result.Error!.Description,
            Answer = request.Answer,
            AttemptsLeft = 3,
        };

        return Results.Ok(response);
    }
}
