using GuessCode.DAL.Models.Enums;

namespace GuessCode.API.Models.V1.User;

public class ProfileInfoDto
{
    public string Username { get; set; } = null!;
    
    public string? AvatarUrl { get; set; }
    
    public string? Description { get; set; }
    
    public ActivityStatus ActivityStatus { get; set; }

    public long UserId { get; set; }
}