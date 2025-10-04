using System.Text;
using System.Text.RegularExpressions;
using Isopoh.Cryptography.Argon2;
using Microsoft.EntityFrameworkCore;
using WebServer.Data;
using WebServer.DTOs.Auth;
using WebServer.Models;
using WebServer.Models.Auth;
using WebServer.Models.Errors;

namespace WebServer.Services;

public class AuthService(JWTHandler jwtHandler, ApplicationDbContext dbContext)
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly JWTHandler _jwtHandler = jwtHandler;

    public async Task<Result<User>> RegisterUserAsync(UserDto userDto)
    {
        try
        {
            if (await _dbContext.Users.AnyAsync(u => u.Email == userDto.Email))
                return Result<User>.Failure(AuthErrors.EmailTaken);

            string pass = userDto.Password;
            string? email = userDto.Email;

            if (string.IsNullOrEmpty(email))
                return Result<User>.Failure(AuthErrors.InvalidEmail);

            var regexMail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regexMail.Match(email);

            if (!match.Success)
                return Result<User>.Failure(AuthErrors.InvalidEmail);

            // TODO add validations of password complexity
            if (string.IsNullOrEmpty(pass) || pass.Length < 8)
                return Result<User>.Failure(AuthErrors.InvalidPassword);

            var hashedPassword = Argon2.Hash(userDto.Password);

            var userId = Guid.NewGuid();

            User newUser = new()
            {
                PasswordHash = hashedPassword,
                Id = userId,
                UserName = userDto.UserName.ToLower(),
                Email = email,
                UserRoles = [new UserRole() { UserId = userId, RoleId = 2 }], // XXX Hardcoded 'User' Role
            };

            _dbContext.Users.Add(newUser);

            await _dbContext.SaveChangesAsync();
            return Result<User>.Success(newUser);
        }
        catch (Exception ex)
        {
            // Log the exception (ex) here
            return Result<User>.Failure(BaseErrors.RegistrationFailed);
        }
    }

    public async Task<Result<TokenResponseDto>> LoginUserAsync(UserDto userDto, Device device)
    {
        var user = await _dbContext
            .Users.Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .ThenInclude(rp => rp.RolePermissions)
            .ThenInclude(p => p.Permission)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == userDto.Email.ToLower());

        bool isCredentialsWrong = false;

        isCredentialsWrong =
            user is null
            || !Argon2.Verify(user.PasswordHash, Encoding.UTF8.GetBytes(userDto.Password));

        if (isCredentialsWrong)
            return Result<TokenResponseDto>.Failure(AuthErrors.InvalidCredentials);

        TokenResponseDto response = await GenerateSessionAndSaveRefreshTokenAsync(user!, device);

        return Result<TokenResponseDto>.Success(response);
    }

    public async Task<bool> ValidateRefreshTokenAsync(RefreshTokenRequestDto dto)
    {
        var session = await _dbContext.Sessions.FirstOrDefaultAsync(s =>
            s.UserId == dto.UserId && s.AccessToken == dto.ExpiredAccessToken
        );

        if (
            session is null
            || session.RefreshToken != dto.RefreshToken
            || session.RefreshTokenExpiryTime <= DateTime.UtcNow
        )
            return false;

        return true;
    }

    public async Task<TokenResponseDto> GenerateSessionAndSaveRefreshTokenAsync(
        User user,
        Device? device = null,
        string? ExpiredAccessToken = null
    )
    {
        string newAccessToken = _jwtHandler.CreateToken(user!);

        var refreshToken = _jwtHandler.GenerateRefreshToken();

        Session? session = await _dbContext.Sessions.FirstOrDefaultAsync(s =>
            s.UserId == user.Id && s.AccessToken == ExpiredAccessToken
        );
        if (session == null)
        {
            session = new Session()
            {
                Id = Guid.NewGuid(),
                AccessToken = newAccessToken,
                UserId = user.Id,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(2),
                DeviceId = device?.DeviceId,
            };
            _dbContext.Sessions.Add(session);
        }
        else
        {
            session.AccessToken = newAccessToken;
            session.RefreshToken = refreshToken;
            session.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(2);

            _dbContext.Sessions.Update(session);
        }

        await _dbContext.SaveChangesAsync();
        return new() { AccessToken = newAccessToken, RefreshToken = refreshToken };
    }

    public async Task<User?> AssignRoleToUserAsync(Guid userID, params int[] rolesId)
    {
        User? user = await _dbContext
            .Users.Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == userID);

        if (user is null)
            return null;

        foreach (var roleId in rolesId)
        {
            if (user.UserRoles.Any(ur => ur.RoleId == roleId))
                continue;

            user.UserRoles.Add(new UserRole() { UserId = userID, RoleId = roleId });
        }

        await _dbContext.SaveChangesAsync();

        return user;
    }
}
