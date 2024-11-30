using System.ComponentModel.DataAnnotations;
using GuessCode.DAL.Contexts;
using GuessCode.DAL.Models.Enums;
using GuessCode.DAL.Models.UserAggregate;
using GuessCode.Domain.Contracts;
using GuessCode.Domain.Scheduled.Requests;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace GuessCode.Domain.Services;

public class UserStatusUpdateService : IUserStatusUpdateService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly GuessContext _context;

    public UserStatusUpdateService(IConnectionMultiplexer redis, GuessContext context)
    {
        _redis = redis;
        _context = context;
    }

    public async Task HandleUserActivityStatusOffline(long userId, CancellationToken cancellationToken)
    {
        var database = _redis.GetDatabase();
        var values = (await database.ListRangeAsync(nameof(UpdateUserActivityStatusesCommand))).Select(value => (long)value);

        if (!values.Contains(userId))
        {
            database.ListRightPush(nameof(UpdateUserActivityStatusesCommand), userId);
        }
    }

    public async Task HandleUserActivityStatusOnline(long userId, CancellationToken cancellationToken)
    {
        var database = _redis.GetDatabase();
        var values = (await database.ListRangeAsync(nameof(UpdateUserActivityStatusesCommand))).Select(value => (long)value);

        if (values.Contains(userId))
        {
            await database.ListRemoveAsync(nameof(UpdateUserActivityStatusesCommand), userId);
        }

        var userProfile = await _context
                              .Set<UserProfile>()
                              .SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken) ??
                          throw new ValidationException($"User profile for {userId} not found");

        userProfile.ActivityStatus = ActivityStatus.Online;
        await _context.SaveChangesAsync(cancellationToken);
    }
    
}