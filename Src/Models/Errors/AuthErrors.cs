namespace WebServer.Models.Errors;

public static class AuthErrors
{
    public static Error EmailTaken => new("EmailTaken", "The email is already taken.");
    public static Error InvalidEmail =>
        new("InvalidEmail", "The mail doesn't have a correct format");
    public static Error InvalidPassword =>
        new("InvalidPassword", "The password length should be at least 8 characters");

    public static Error InvalidCredentials =>
        new("InvalidCredentials", "The user or the password is wrong");
}
