using GuessCode.DAL.Models.UserAggregate.Enums;

namespace GuessCode.DAL.Models.UserAggregate;

public class MentorRequest
{
    public long MentorId { get; set; }
    
    public Mentor Mentor { get; set; }
    
    public long UserId { get; set; }
    
    public User User { get; set; }
    
    public MentorRequestStatus Status { get; set; }
}