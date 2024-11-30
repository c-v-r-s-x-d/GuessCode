using AutoMapper;
using GuessCode.API.Models.V1.Leaderboard;
using GuessCode.Domain.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace GuessCode.API.Controllers;

[ApiController]
[Route("api/leaderboard")]
public class LeaderboardController : BaseGuessController
{
    private readonly ILeaderboardService _leaderboardService;
    private readonly IMapper _mapper;

    public LeaderboardController(ILeaderboardService leaderboardService, IMapper mapper)
    {
        _leaderboardService = leaderboardService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IReadOnlyCollection<LeaderboardPositionDto>> GetLeaderboard(CancellationToken cancellationToken)
    {
        var leaderboard = await _leaderboardService.GetLeaderboard(cancellationToken);
        return _mapper.Map<IReadOnlyCollection<LeaderboardPositionDto>>(leaderboard);
    }
}