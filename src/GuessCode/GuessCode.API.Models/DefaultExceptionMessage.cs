namespace GuessCode.API.Models;

public class DefaultExceptionMessage
{
    public string ErrorMessage { get; set; } = null!;

    public string? ErrorSource { get; set; }
}