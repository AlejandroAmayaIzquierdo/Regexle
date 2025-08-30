namespace WebServer.DTOs.Challenges;

public class SubmitResponseDto
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Answer { get; set; }
    public int AttemptsLeft { get; set; }
}
