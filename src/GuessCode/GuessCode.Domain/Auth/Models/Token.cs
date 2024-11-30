namespace GuessCode.Domain.Auth.Models;

public class Token
{
    public string? AccessToken { get; set; }
    
    public long? UserId { get; set; }
}