using System.ComponentModel.DataAnnotations;
using GuessCode.DAL.Contexts;
using GuessCode.DAL.Models.UserAggregate;
using GuessCode.Domain.Rating.Contracts;
using GuessCode.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace GuessCode.Domain.Rating.Services;

public class RatingService : IRatingService
{
    private readonly GuessContext _context;

    public RatingService(GuessContext context)
    {
        _context = context;
    }

    public async Task AdjustRatingAndUpdateRank(RatingChange ratingChange)
    {
        var user = await _context
                       .Set<User>()
                       .SingleOrDefaultAsync(x => x.Id == ratingChange.UserId) ??
                   throw new ValidationException($"User {ratingChange.UserId} not found");
        
        user.RatingChanges.Add(ratingChange);
        user.Rating += ratingChange.RatingChangeValue;
        UpdateRankIfNeeded(user);
        await _context.SaveChangesAsync();
    }

    private static void UpdateRankIfNeeded(User user)
    {
        var currentRatingRank = RankUtils.CheckRank(user.Rating);

        if (user.Rank != currentRatingRank)
        {
            user.Rank = currentRatingRank;
        }
    }
}