using System.ComponentModel.DataAnnotations;
using GuessCode.DAL.Contexts;
using GuessCode.DAL.Models.KataAggregate;
using GuessCode.DAL.Models.UserAggregate;
using GuessCode.Domain.Contracts;
using GuessCode.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GuessCode.Domain.Services;

public class KataSolveService : IKataSolveService
{
    private readonly GuessContext _context;

    public KataSolveService(GuessContext context)
    {
        _context = context;
    }

    public async Task<KataSolveResult> SolveKata(long userId, KataAnswer kataAnswer, CancellationToken cancellationToken)
    {
        var kata = await _context
                       .Set<Kata>()
                       .SingleOrDefaultAsync(x => x.Id == kataAnswer.KataId, cancellationToken) ??
                   throw new ValidationException("Kata not found");

        var isAnswerCorrect =
            kata.KataJsonContent.AnswerOptions.Single(x => x.OptionId == kataAnswer.OptionId).IsCorrect;

        if (isAnswerCorrect)
        {
            var user = await _context
                .Set<User>()
                .SingleAsync(x => x.Id == userId, cancellationToken);
            user.Rating += 25;
            await _context.SaveChangesAsync(cancellationToken);
        }

        return new KataSolveResult
        {
            IsAnswerCorrect = isAnswerCorrect
        };
    }
}