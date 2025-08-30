using Microsoft.EntityFrameworkCore;
using WebServer.Models;
using WebServer.Models.Auth;
using WebServer.Models.Challenges;

namespace WebServer.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    // Auth
    public DbSet<User> Users { get; set; }
    public DbSet<Session> Sessions { get; set; }

    public DbSet<Device> Devices { get; set; }

    // Roles
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<Permission> Permissions { get; set; }

    // Challenges
    public DbSet<Challenge> Challenges { get; set; }
    public DbSet<ChallengeSchedule> ChallengeSchedules { get; set; }
    public DbSet<TestCase> TestCases { get; set; }

    public DbSet<UserAttempt> UserAttempts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed roles
        modelBuilder
            .Entity<Role>()
            .HasData(
                new Role
                {
                    Id = 1,
                    Name = "Admin",
                    Description = "Administrator role with full access",
                    Active = true,
                },
                new Role
                {
                    Id = 2,
                    Name = "User",
                    Description = "Regular user that can play daily challenges",
                    Active = true,
                },
                new Role
                {
                    Id = 3,
                    Name = "Moderator",
                    Description = "Moderator that can create/edit challenges but not full admin",
                    Active = true,
                }
            );
        modelBuilder
            .Entity<Permission>()
            .HasData(
                new Permission { Id = (int)PermissionTypes.AdminAccess, Name = "AdminAccess" },
                new Permission { Id = (int)PermissionTypes.UserAccess, Name = "UserAccess" },
                new Permission { Id = (int)PermissionTypes.PlayChallenge, Name = "PlayChallenge" },
                new Permission { Id = (int)PermissionTypes.ViewHistory, Name = "ViewHistory" },
                new Permission
                {
                    Id = (int)PermissionTypes.ViewLeaderboard,
                    Name = "ViewLeaderboard",
                },
                new Permission
                {
                    Id = (int)PermissionTypes.CreateChallenge,
                    Name = "CreateChallenge",
                },
                new Permission { Id = (int)PermissionTypes.EditChallenge, Name = "EditChallenge" },
                new Permission
                {
                    Id = (int)PermissionTypes.DeleteChallenge,
                    Name = "DeleteChallenge",
                },
                new Permission
                {
                    Id = (int)PermissionTypes.ViewFutureChallenges,
                    Name = "ViewFutureChallenges",
                }
            );

        modelBuilder
            .Entity<RolePermission>()
            .HasData(
                // Admin (full)
                new RolePermission
                {
                    Id = 1,
                    RoleId = 1,
                    PermissionId = (int)PermissionTypes.AdminAccess,
                },
                new RolePermission
                {
                    Id = 2,
                    RoleId = 1,
                    PermissionId = (int)PermissionTypes.UserAccess,
                },
                new RolePermission
                {
                    Id = 3,
                    RoleId = 1,
                    PermissionId = (int)PermissionTypes.PlayChallenge,
                },
                new RolePermission
                {
                    Id = 4,
                    RoleId = 1,
                    PermissionId = (int)PermissionTypes.ViewHistory,
                },
                new RolePermission
                {
                    Id = 5,
                    RoleId = 1,
                    PermissionId = (int)PermissionTypes.ViewLeaderboard,
                },
                new RolePermission
                {
                    Id = 6,
                    RoleId = 1,
                    PermissionId = (int)PermissionTypes.CreateChallenge,
                },
                new RolePermission
                {
                    Id = 7,
                    RoleId = 1,
                    PermissionId = (int)PermissionTypes.EditChallenge,
                },
                new RolePermission
                {
                    Id = 8,
                    RoleId = 1,
                    PermissionId = (int)PermissionTypes.DeleteChallenge,
                },
                new RolePermission
                {
                    Id = 9,
                    RoleId = 1,
                    PermissionId = (int)PermissionTypes.ViewFutureChallenges,
                },
                // User (básico)
                new RolePermission
                {
                    Id = 10,
                    RoleId = 2,
                    PermissionId = (int)PermissionTypes.UserAccess,
                },
                new RolePermission
                {
                    Id = 11,
                    RoleId = 2,
                    PermissionId = (int)PermissionTypes.PlayChallenge,
                },
                new RolePermission
                {
                    Id = 12,
                    RoleId = 2,
                    PermissionId = (int)PermissionTypes.ViewHistory,
                },
                new RolePermission
                {
                    Id = 13,
                    RoleId = 2,
                    PermissionId = (int)PermissionTypes.ViewLeaderboard,
                },
                // Moderator (curador de retos)
                new RolePermission
                {
                    Id = 14,
                    RoleId = 3,
                    PermissionId = (int)PermissionTypes.UserAccess,
                },
                new RolePermission
                {
                    Id = 15,
                    RoleId = 3,
                    PermissionId = (int)PermissionTypes.PlayChallenge,
                },
                new RolePermission
                {
                    Id = 16,
                    RoleId = 3,
                    PermissionId = (int)PermissionTypes.ViewHistory,
                },
                new RolePermission
                {
                    Id = 17,
                    RoleId = 3,
                    PermissionId = (int)PermissionTypes.ViewLeaderboard,
                },
                new RolePermission
                {
                    Id = 18,
                    RoleId = 3,
                    PermissionId = (int)PermissionTypes.CreateChallenge,
                },
                new RolePermission
                {
                    Id = 19,
                    RoleId = 3,
                    PermissionId = (int)PermissionTypes.EditChallenge,
                },
                new RolePermission
                {
                    Id = 20,
                    RoleId = 3,
                    PermissionId = (int)PermissionTypes.ViewFutureChallenges,
                }
            );

        var challengeEmailId = Guid.NewGuid();
        modelBuilder
            .Entity<Challenge>()
            .HasData(
                new Challenge
                {
                    Id = challengeEmailId,
                    Title = "Validar Email",
                    Description = "Encuentra un email válido (ejemplo: user@example.com).",
                    Solution = @"^[\w\.-]+@[\w\.-]+\.\w+$",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                }
            );

        modelBuilder
            .Entity<TestCase>()
            .HasData(
                new TestCase
                {
                    Id = Guid.NewGuid(),
                    ChallengeId = challengeEmailId,
                    Text = "user@example.com",
                    IsMatch = true,
                },
                new TestCase
                {
                    Id = Guid.NewGuid(),
                    ChallengeId = challengeEmailId,
                    Text = "john.doe@domain.co.uk",
                    IsMatch = true,
                },
                new TestCase
                {
                    Id = Guid.NewGuid(),
                    ChallengeId = challengeEmailId,
                    Text = "not-an-email",
                    IsMatch = false,
                }
            );
    }
}
