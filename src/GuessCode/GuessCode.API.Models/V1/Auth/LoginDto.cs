namespace GuessCode.API.Models.V1.Auth;

public class LoginDto
{
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;
}