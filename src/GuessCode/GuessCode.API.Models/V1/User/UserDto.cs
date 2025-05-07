using GuessCode.DAL.Models.Enums;

namespace GuessCode.API.Models.V1.User;

public class UserDto
{
    public long Id { get; set; }
    
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;
    
    public DateTime RegistrationDate { get; set; }
    
    public Rank Rank { get; set; }
    
    public long Rating { get; set; }
    
    public long? GitHubProfileId { get; set; }

    public long? UserProfileId { get; set; }

    public long? RoleId { get; set; }
    
    public long? MentorId { get; set; }
}