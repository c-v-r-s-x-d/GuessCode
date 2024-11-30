using GuessCode.DAL.Models.Enums;

namespace GuessCode.API.Models.V1.Leaderboard;

public class LeaderboardPositionDto
{
    public long UserId { get; set; }

    public string Username { get; set; } = null!;
    
    public Rank Rank { get; set; }
    
    public long Rating { get; set; }
}