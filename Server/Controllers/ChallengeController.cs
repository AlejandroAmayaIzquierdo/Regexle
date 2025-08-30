using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServer.DTOs.Challenges;
using WebServer.Services;

namespace WebServer.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = "UserAccess")]
public class ChallengeController(ChallengeService challengeService) : ControllerBase
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
    public async Task<IResult> SubmitChallenge(SubmitRequest request)
    {
        var challenge = await _challengeService.GetTodayChallengeAsync();
        var result = await _challengeService.CheckRegex(request.Answer, challenge);

        if (result.IsSuccess)
            return Results.Ok(
                new SubmitResponseDto
                {
                    IsSuccess = true,
                    Answer = request.Answer,
                    AttemptsLeft = 3,
                }
            );

        int statusCode = result.Error!.Code switch
        {
            "ChallengeNotFound" => 404,
            "InvalidRegex" => 400,
            _ => 200,
        };

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
