using GuessCode.DAL.Models.BaseModels;

namespace GuessCode.DAL.Models.UserAggregate;

public class GitHubProfile : BaseEntity<long>
{
    public string Login { get; set; }
    
    public long UserId { get; set; }
    public User User { get; set; }
    
    // For another github information...
}