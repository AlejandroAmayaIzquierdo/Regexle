namespace WebServer.Models.Auth;

public enum PermissionTypes
{
    AdminAccess = 1,
    UserAccess = 2,

    // Daily Challenge
    PlayChallenge = 3,
    ViewHistory = 4,
    ViewLeaderboard = 5,

    // Admin / Moderación
    CreateChallenge = 6,
    EditChallenge = 7,
    DeleteChallenge = 8,
    ViewFutureChallenges = 9,
}
