using GuessCode.DAL.Contexts;
using GuessCode.DAL.Models.UserAggregate;
using GuessCode.Domain.Contracts;
using GuessCode.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GuessCode.Domain.Services;

public class LeaderboardService : ILeaderboardService
{
    private readonly GuessContext _context;
    private const int LeaderboardPositionsCount = 100;

    public LeaderboardService(GuessContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<LeaderboardPosition>> GetLeaderboard(CancellationToken cancellationToken)
    {
        var leaderboard = await _context
            .Set<User>()
            .AsNoTracking()
            .Where(x => x.Id != SystemUserIds.System)
            .OrderByDescending(x => x.Rating)
            .Take(LeaderboardPositionsCount)
            .Select(x => new LeaderboardPosition
            {
                UserId = x.Id,
                Username = x.Username,
                Rank = x.Rank,
                Rating = x.Rating
            })
            .ToListAsync(cancellationToken);

        return leaderboard;
    }
}