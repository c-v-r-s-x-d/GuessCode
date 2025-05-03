using GuessCode.DAL.Models.BaseModels;
using GuessCode.DAL.Models.UserAggregate.Enums;

namespace GuessCode.DAL.Models.UserAggregate;

public class RatingChange : BaseEntity<long>
{
    public long UserId { get; set; }
    
    public User User { get; set; }
    
    public long RatingChangeValue { get; set; }
    
    public RatingChangeReason ChangeReason { get; set; }
    
    public long ChangedBy { get; set; }
}