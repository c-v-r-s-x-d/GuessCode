using GuessCode.DAL.Models.UserAggregate;

namespace GuessCode.Domain.Rating.Contracts;

public interface IRatingService
{
    Task AdjustRatingAndUpdateRank(RatingChange ratingChange);
}