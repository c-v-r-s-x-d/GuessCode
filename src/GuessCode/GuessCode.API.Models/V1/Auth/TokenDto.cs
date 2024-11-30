namespace GuessCode.API.Models.V1.Auth;

public class TokenDto
{
    public string? AccessToken { get; set; }
    
    public long? UserId { get; set; }
}