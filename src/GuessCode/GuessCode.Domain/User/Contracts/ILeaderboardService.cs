using GuessCode.Domain.Models;

namespace GuessCode.Domain.Contracts;

public interface ILeaderboardService
{
    Task<IReadOnlyCollection<LeaderboardPosition>> GetLeaderboard(CancellationToken cancellationToken);
}