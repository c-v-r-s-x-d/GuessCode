namespace GuessCode.API.Models.V1.Auth;

public class RegisterDto
{
    public string Email { get; set; } = null!;
    
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;
}