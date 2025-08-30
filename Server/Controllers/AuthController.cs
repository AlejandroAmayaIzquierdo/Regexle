using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServer.DTOs.Auth;
using WebServer.Models;
using WebServer.Models.Auth;
using WebServer.Services;

namespace WebServer.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(
    AuthService authService,
    DeviceService deviceService,
    UserService userService
) : ControllerBase
{
    private readonly AuthService _authService = authService;
    private readonly DeviceService _deviceService = deviceService;
    private readonly UserService _userService = userService;

    [HttpPost("register")]
    public async Task<IResult> RegisterAsync([FromBody] UserDto req)
    {
        if (string.IsNullOrEmpty(req.UserName) || string.IsNullOrEmpty(req.Email))
            return Results.BadRequest("The username or the mail need to be provide");
        Result<User> result = await _authService.RegisterUserAsync(req);

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

    [HttpPost("login")]
    public async Task<IResult> LoginAsync([FromBody] UserDto req)
    {
        await Task.CompletedTask;
        var userAgent = HttpContext.Request.Headers.UserAgent;
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

        if (string.IsNullOrEmpty(userAgent) || string.IsNullOrEmpty(ipAddress))
            return Results.Problem(
                "User-Agent or IP Address is missing",
                statusCode: StatusCodes.Status400BadRequest
            );

        var (device, _) = await _deviceService.RegisterDevice(
            new() { IpAddress = ipAddress, UserAgent = userAgent! }
        );

        if (device is null)
            return Results.Problem(
                "Failed to register device",
                statusCode: StatusCodes.Status500InternalServerError
            );

        var result = await _authService.LoginUserAsync(req, device);

        if (result.IsFailure)
            return Results.Problem(
                result.Error!.Description,
                statusCode: StatusCodes.Status500InternalServerError
            );

        var tokenResponse = result.Value;

        return Results.Ok(tokenResponse);
    }

    [HttpPost("refresh-token")]
    public async Task<IResult> RefreshTokenAsync([FromBody] RefreshTokenRequestDto req)
    {
        var userAgent = HttpContext.Request.Headers.UserAgent;
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

        if (string.IsNullOrEmpty(userAgent) || string.IsNullOrEmpty(ipAddress))
            return Results.Problem(
                "User-Agent or IP Address is missing",
                statusCode: StatusCodes.Status400BadRequest
            );

        var deviceRequest = _deviceService.BuildDevice(
            new() { IpAddress = ipAddress, UserAgent = userAgent! }
        );

        bool isValidDevice = await _deviceService.ValidateDevice(
            deviceRequest,
            req.ExpiredAccessToken
        );

        if (!isValidDevice)
            return Results.Problem(
                "Unauthorized device",
                statusCode: StatusCodes.Status400BadRequest
            );

        var user = await _userService.GetUserByIdAsync(req.UserId);
        var isTokenValid = await _authService.ValidateRefreshTokenAsync(req);

        if (user is null || !isTokenValid)
            return Results.Unauthorized();

        return Results.Ok(
            await _authService.GenerateSessionAndSaveRefreshTokenAsync(
                user,
                deviceRequest,
                req.ExpiredAccessToken
            )
        );
    }

    [HttpPost("login/guest")]
    public async Task<IResult> Guess()
    {
        await Task.CompletedTask;

        var result = await _authService.RegisterUserAsync(Constants.GuessUser);

        if (result.IsFailure)
            return Results.Problem("Failed to register guess user", statusCode: 500);

        var user = await _userService.GetUserByIdAsync(result.Value!.Id);

        if (user is null)
            return Results.Problem("Failed to retrieve guess user", statusCode: 500);

        var sessionResult = await _authService.GenerateSessionAndSaveRefreshTokenAsync(user);

        return Results.Ok(new { accessToken = sessionResult.AccessToken });
    }
}
