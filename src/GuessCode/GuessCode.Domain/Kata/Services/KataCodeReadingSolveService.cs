using System.ComponentModel.DataAnnotations;
using GuessCode.DAL.Contexts;
using GuessCode.DAL.Models.Enums;
using GuessCode.DAL.Models.KataAggregate;
using GuessCode.DAL.Models.UserAggregate;
using GuessCode.DAL.Models.UserAggregate.Enums;
using GuessCode.Domain.Contracts;
using GuessCode.Domain.Models;
using GuessCode.Domain.Rating.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GuessCode.Domain.Services;

public class KataCodeReadingSolveService : IKataCodeReadingSolveService
{
    private readonly GuessContext _context;
    private readonly IRatingService _ratingService;

    public KataCodeReadingSolveService(GuessContext context, IRatingService ratingService)
    {
        _context = context;
        _ratingService = ratingService;
    }

    public async Task<KataCodeReadingSolveResult> SolveKata(long userId, KataCodeReadingAnswer kataCodeReadingAnswer, CancellationToken cancellationToken)
    {
        var kata = await _context
                       .Set<Kata>()
                       .SingleOrDefaultAsync(x => x.Id == kataCodeReadingAnswer.KataId, cancellationToken) ??
                   throw new ValidationException("Kata not found");
        
        var user = await _context
            .Set<User>()
            .Include(x => x.ResolvedKatas)
            .SingleAsync(x => x.Id == userId, cancellationToken);

        if (user.ResolvedKatas.Select(x => x.Id).Contains(kata.Id))
        {
            throw new ValidationException("Kata already resolved");
        }

        if (kata.KataType is not KataType.CodeReading)
        {
            throw new ValidationException("This solve method is only supported for KataType code reading");
        }

        var isAnswerCorrect =
            kata.KataJsonContent.AnswerOptions.Single(x => x.OptionId == kataCodeReadingAnswer.OptionId).IsCorrect;

        if (isAnswerCorrect)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            
            await _ratingService.AdjustRatingAndUpdateRank(new RatingChange
            {
                UserId = userId,
                RatingChangeValue = kata.PointsReward,
                ChangeReason = RatingChangeReason.KataSolved,
                ChangedBy = SystemUserIds.System
            });
            user.ResolvedKatas.Add(kata);
            await _context.SaveChangesAsync(cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);
        }

        return new KataCodeReadingSolveResult
        {
            IsAnswerCorrect = isAnswerCorrect
        };
    }
}