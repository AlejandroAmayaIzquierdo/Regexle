using WebServer.Models.Auth;

namespace WebServer.Endpoints;

public static class AuthController
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var authRoutes = app.MapGroup("/auth");

        authRoutes
            .MapPost("/register", AuthHandlers.RegisterAsync)
            .WithName("Register")
            .WithTags("Authentication")
            .Produces<User>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

        authRoutes
            .MapPost("/login", AuthHandlers.LoginAsync)
            .WithName("Login")
            .WithTags("Authentication")
            .Produces<User>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        authRoutes
            .MapPost("/refresh-token", AuthHandlers.RefreshTokenAsync)
            .WithName("RefreshToken")
            .WithTags("Authentication")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError);
    }
}
