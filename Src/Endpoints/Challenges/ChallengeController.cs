using WebServer.Models.Auth;
using WebServer.Models.Challenges;

namespace WebServer.Endpoints;

public static class ChallengeController
{
    public static void MapChallengeEndpoints(this IEndpointRouteBuilder app)
    {
        var challengeRoutes = app.MapGroup("/challenges");

        challengeRoutes
            .MapPost("/create", ChallengeHandlers.CreateAsync)
            .WithName("CreateChallenge")
            .WithTags("Challenges")
            .Produces<Challenge>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(policy => policy.RequirePermissions(PermissionTypes.UserAccess));

        // challengeRoutes
        //     .MapGet("/{id}", ChallengeHandlers.GetByIdAsync)
        //     .WithName("GetChallengeById")
        //     .WithTags("Challenges")
        //     .Produces<Challenge>(StatusCodes.Status200OK)
        //     .Produces(StatusCodes.Status404NotFound)
        //     .Produces(StatusCodes.Status500InternalServerError);

        // challengeRoutes
        //     .MapPut("/{id}", ChallengeHandlers.UpdateAsync)
        //     .WithName("UpdateChallenge")
        //     .WithTags("Challenges")
        //     .Produces<Challenge>(StatusCodes.Status200OK)
        //     .Produces(StatusCodes.Status404NotFound)
        //     .Produces(StatusCodes.Status500InternalServerError);

        // challengeRoutes
        //     .MapDelete("/{id}", ChallengeHandlers.DeleteAsync)
        //     .WithName("DeleteChallenge")
        //     .WithTags("Challenges")
        //     .Produces(StatusCodes.Status204NoContent)
        //     .Produces(StatusCodes.Status404NotFound)
        //     .Produces(StatusCodes.Status500InternalServerError);
    }
}
