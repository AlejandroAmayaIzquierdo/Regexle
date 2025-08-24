namespace WebServer.Models.Errors;

public static class BaseErrors
{
    public static Error RegistrationFailed =>
        new("RegistrationFailed", "User registration failed.");
}
