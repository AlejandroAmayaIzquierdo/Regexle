using Microsoft.AspNetCore.Mvc;
using WebServer.DTOs.Auth;
using WebServer.Models;
using WebServer.Models.Auth;
using WebServer.Services;

namespace WebServer.Endpoints;

public static class AuthHandlers
{
    public static async Task<IResult> RegisterAsync(
        [FromBody] UserDto req,
        [FromServices] AuthService service
    )
    {
        if (string.IsNullOrEmpty(req.UserName) || string.IsNullOrEmpty(req.Email))
            return Results.BadRequest("The username or the mail need to be provide");
        Result<User> result = await service.RegisterUserAsync(req);

        if (result.IsFailure)
        {
            var error = result.Error!;
            var statusCode = error.Code switch
            {
                "EmailTaken" or "InvalidEmail" or "InvalidPassword" =>
                    StatusCodes.Status400BadRequest,
                "RegistrationFailed" => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError,
            };

            return Results.Problem(error.Description, statusCode: statusCode);
        }

        return Results.Ok(result.Value);
    }

    public static async Task<IResult> LoginAsync(
        [FromBody] UserDto req,
        [FromServices] AuthService authService,
        [FromServices] DeviceService deviceService,
        HttpContext context
    )
    {
        await Task.CompletedTask;
        var userAgent = context.Request.Headers.UserAgent;
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();

        if (string.IsNullOrEmpty(userAgent) || string.IsNullOrEmpty(ipAddress))
            return Results.Problem(
                "User-Agent or IP Address is missing",
                statusCode: StatusCodes.Status400BadRequest
            );

        var (device, _) = await deviceService.RegisterDevice(
            new() { IpAddress = ipAddress, UserAgent = userAgent! }
        );

        if (device is null)
            return Results.Problem(
                "Failed to register device",
                statusCode: StatusCodes.Status500InternalServerError
            );

        var result = await authService.LoginUserAsync(req, device);

        if (result.IsFailure)
            return Results.Problem(
                result.Error!.Description,
                statusCode: StatusCodes.Status500InternalServerError
            );

        var tokenResponse = result.Value;

        return Results.Ok(tokenResponse);
    }

    public static async Task<IResult> RefreshTokenAsync(
        [FromBody] RefreshTokenRequestDto req,
        [FromServices] UserService userService,
        [FromServices] AuthService authService,
        [FromServices] DeviceService deviceService,
        HttpContext context
    )
    {
        var userAgent = context.Request.Headers.UserAgent;
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();

        if (string.IsNullOrEmpty(userAgent) || string.IsNullOrEmpty(ipAddress))
            return Results.Problem(
                "User-Agent or IP Address is missing",
                statusCode: StatusCodes.Status400BadRequest
            );

        var deviceRequest = deviceService.BuildDevice(
            new() { IpAddress = ipAddress, UserAgent = userAgent! }
        );

        bool isValidDevice = await deviceService.ValidateDevice(
            deviceRequest,
            req.ExpiredAccessToken
        );

        if (!isValidDevice)
            return Results.Problem(
                "Unauthorized device",
                statusCode: StatusCodes.Status400BadRequest
            );

        var user = await userService.GetUserByIdAsync(req.UserId);
        var isTokenValid = await authService.ValidateRefreshTokenAsync(req);

        if (user is null || !isTokenValid)
            return Results.Unauthorized();

        return Results.Ok(
            await authService.GenerateSessionAndSaveRefreshTokenAsync(
                user,
                deviceRequest,
                req.ExpiredAccessToken
            )
        );
    }
}
