using GuessCode.DAL.Models.BaseModels;
using GuessCode.DAL.Models.Enums;
using GuessCode.DAL.Models.UserAggregate.Enums;

namespace GuessCode.DAL.Models.UserAggregate;

public class Mentor : BaseEntity<long>
{
    public long UserId { get; set; }
    
    public User User { get; set; }
    
    public long Experience { get; set; }
    
    public List<ProgrammingLanguage> ProgrammingLanguages { get; set; }
    
    public MentorAvailability Availability { get; set; }
    
    public string About { get; set; }
    
    public decimal Rating { get; set; }
    
    public bool IsApproved { get; set; }
    
    public List<User> Mentees { get; set; }
}