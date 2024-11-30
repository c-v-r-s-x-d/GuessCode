using System.ComponentModel.DataAnnotations;
using GuessCode.DAL.Contexts;
using GuessCode.DAL.Models.Enums;
using GuessCode.DAL.Models.UserAggregate;
using GuessCode.Domain.Contracts;
using GuessCode.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GuessCode.Domain.Services;

public class ProfileInfoService : IProfileInfoService
{
    private readonly GuessContext _context;

    public ProfileInfoService(GuessContext context)
    {
        _context = context;
    }

    public async Task<ProfileInfo> GetUserProfileInfo(long userId, CancellationToken cancellationToken)
    {
        var user = await _context
                       .Set<User>()
                       .AsNoTracking()
                       .SingleOrDefaultAsync(x => x.Id == userId, cancellationToken) ??
                   throw new ValidationException($"Profile for user {userId} not found");
        
        var userProfile = await _context
            .Set<UserProfile>()
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken) ??
        throw new ValidationException($"Profile for user {userId} not found");

        var aggregatedProfileInfo = GetAggregatedProfileInfo(user, userProfile);
        return aggregatedProfileInfo;
    }

    private static ProfileInfo GetAggregatedProfileInfo(User user, UserProfile userProfile)
    {
        return new ProfileInfo
        {
            Username = user.Username,
            AvatarUrl = userProfile.AvatarUrl,
            Description = userProfile.Description,
            ActivityStatus = userProfile.ActivityStatus,
            UserId = user.Id
        };
    }
}