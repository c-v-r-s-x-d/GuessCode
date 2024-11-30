using System.ComponentModel.DataAnnotations;
using GuessCode.DAL.Contexts;
using GuessCode.DAL.Models.KataAggregate;
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

    public async Task<KataSolveResult> SolveKata(KataAnswer kataAnswer, CancellationToken cancellationToken)
    {
        var kata = await _context
                       .Set<Kata>()
                       .SingleOrDefaultAsync(x => x.Id == kataAnswer.KataId, cancellationToken) ??
                   throw new ValidationException("Kata not found");

        var isAnswerCorrect =
            kata.KataJsonContent.AnswerOptions.Single(x => x.OptionId == kataAnswer.OptionId).IsCorrect;

        return new KataSolveResult
        {
            IsAnswerCorrect = isAnswerCorrect
        };
    }
}