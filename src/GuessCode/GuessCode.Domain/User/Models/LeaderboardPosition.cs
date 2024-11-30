using GuessCode.DAL.Models.Enums;

namespace GuessCode.Domain.Models;

public class LeaderboardPosition
{
    public long UserId { get; set; }

    public string Username { get; set; } = null!;
    
    public Rank Rank { get; set; }
    
    public long Rating { get; set; }
}