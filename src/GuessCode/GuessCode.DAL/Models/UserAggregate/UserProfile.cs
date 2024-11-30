using GuessCode.DAL.Models.BaseModels;
using GuessCode.DAL.Models.Enums;

namespace GuessCode.DAL.Models.UserAggregate;

public class UserProfile : BaseEntity<long>
{
    public string AvatarUrl { get; set; }
    
    public string Description { get; set; }
    
    public ActivityStatus ActivityStatus { get; set; }

    public long UserId { get; set; }
    public User User { get; set; }
}