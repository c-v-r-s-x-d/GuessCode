using GuessCode.DAL.Models.BaseModels;
using GuessCode.DAL.Models.Enums;
using GuessCode.DAL.Models.KataAggregate;
using GuessCode.DAL.Models.RoleAggregate;

namespace GuessCode.DAL.Models.UserAggregate;

public class User : BaseEntity<long>
{
    public string Username { get; set; }
    
    public string Password { get; set; }
    
    public string Email { get; set; }
    
    public DateTime RegistrationDate { get; set; }
    
    public Rank Rank { get; set; }
    
    public long Rating { get; set; }
    
    public long? GitHubProfileId { get; set; }
    public GitHubProfile GitHubProfile { get; set; }
    
    
    public long? UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; }
    
    public List<Kata> AuthoredKatas { get; set; }
    
    public List<Kata> ResolvedKatas { get; set; }
    
    public List<RatingChange> RatingChanges { get; set; }
    
    public long? MentorId { get; set; }
    
    public Mentor Mentor { get; set; }
    
    public long? RoleId { get; set; }
    public Role Role { get; set; }
}