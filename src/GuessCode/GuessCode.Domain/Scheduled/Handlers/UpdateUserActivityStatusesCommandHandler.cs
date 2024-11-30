using GuessCode.DAL.Contexts;
using GuessCode.DAL.Models.Enums;
using GuessCode.DAL.Models.UserAggregate;
using GuessCode.Domain.Scheduled.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace GuessCode.Domain.Scheduled.Handlers;

public class UpdateUserActivityStatusesCommandHandler : IRequestHandler<UpdateUserActivityStatusesCommand>
{
    private readonly IConnectionMultiplexer _redis;
    private readonly GuessContext _context;
    
    public UpdateUserActivityStatusesCommandHandler(IConnectionMultiplexer redis, GuessContext context)
    {
        _redis = redis;
        _context = context;
    }

    public async Task Handle(UpdateUserActivityStatusesCommand request, CancellationToken cancellationToken)
    {
        var database = _redis.GetDatabase();

        var updateRequest = await database.ListRangeAsync(nameof(UpdateUserActivityStatusesCommand));

        if (updateRequest.Length == 0)
        {
            return;
        }

        var userIdsToUpdate = updateRequest.Select(value => (long)value).ToArray();

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        var userProfilesToUpdate = await _context
            .Set<UserProfile>()
            .Where(x => userIdsToUpdate.Contains(x.UserId))
            .ToDictionaryAsync(x => x.UserId, cancellationToken);

        foreach (var userId in userIdsToUpdate)
        {
            if (userProfilesToUpdate.TryGetValue(userId, out var userProfile))
            {
                userProfile.ActivityStatus = ActivityStatus.Offline;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        await database.KeyDeleteAsync(nameof(UpdateUserActivityStatusesCommand));
    }
}