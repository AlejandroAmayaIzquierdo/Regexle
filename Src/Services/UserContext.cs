using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using WebServer.Data;
using WebServer.Models.Auth;

namespace WebServer.Services;

public class UserContext(IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<User?> GetUser(bool loadFromDb = false)
    {
        var context = _httpContextAccessor.HttpContext;

        if (context == null)
            return null;

        var authHeader = context.Request.Headers.Authorization.ToString();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            return null;

        var token = authHeader.Split(" ")[1];
        var handler = new JwtSecurityTokenHandler();

        if (handler.ReadToken(token) is not JwtSecurityToken jwtToken)
            return null;

        var userIdClaim = jwtToken
            .Claims.FirstOrDefault(c =>
                c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
            )
            ?.Value;
        var userNameClaim = jwtToken
            .Claims.FirstOrDefault(c =>
                c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
            )
            ?.Value;

        if (!Guid.TryParse(userIdClaim, out var userId))
            return null;
        var user = new User() { Id = userId, UserName = userNameClaim ?? string.Empty };

        if (loadFromDb)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        return new User { Id = userId, UserName = userNameClaim ?? string.Empty };
    }
}
