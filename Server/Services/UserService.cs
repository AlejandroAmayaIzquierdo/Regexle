using Microsoft.EntityFrameworkCore;
using WebServer.Data;
using WebServer.Models.Auth;

namespace WebServer.Services;

public class UserService(ApplicationDbContext dbContext)
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _dbContext
            .Users.Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .ThenInclude(rp => rp.RolePermissions)
            .ThenInclude(p => p.Permission)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
}
