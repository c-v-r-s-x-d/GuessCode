using GuessCode.DAL.Models.Enums;
using GuessCode.DAL.Models.UserAggregate.Enums;

namespace GuessCode.API.Models.V1.User;

public class MentorDto
{
    public long UserId { get; set; }
    
    public long Experience { get; set; }
    
    public List<ProgrammingLanguage> ProgrammingLanguages { get; set; }
    
    public MentorAvailability Availability { get; set; }
    
    public string About { get; set; }
    
    public decimal Rating { get; set; }
}