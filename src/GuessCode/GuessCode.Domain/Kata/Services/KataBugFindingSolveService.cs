using System.ComponentModel.DataAnnotations;
using GuessCode.DAL.Contexts;
using GuessCode.DAL.Models.Enums;
using GuessCode.DAL.Models.KataAggregate;
using GuessCode.Domain.Contracts;
using GuessCode.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GuessCode.Domain.Services;

public class KataBugFindingSolveService : IKataBugFindingSolveService
{
    private readonly IKataCodeExecutionService _kataCodeExecutionService;
    private readonly GuessContext _context;

    public KataBugFindingSolveService(GuessContext context, IKataCodeExecutionService kataCodeExecutionService)
    {
        _context = context;
        _kataCodeExecutionService = kataCodeExecutionService;
    }

    public async Task<KataBugFindingSolveResult> SolveKata(long userId, KataBugFindingAnswer kataCodeReadingAnswer, CancellationToken cancellationToken)
    {
        var kata = await _context
                       .Set<Kata>()
                       .AsNoTracking()
                       .SingleOrDefaultAsync(k => k.Id == kataCodeReadingAnswer.KataId, cancellationToken) ??
                   throw new ValidationException($"Kata {kataCodeReadingAnswer.KataId} not found");

        if (kata.KataType is not KataType.BugFinding)
        {
            throw new ValidationException($"This solve service is only for KataType {KataType.BugFinding}");
        }

        await _kataCodeExecutionService.ScheduleCodeExecution(userId, kataCodeReadingAnswer.KataId,
            kataCodeReadingAnswer.SourceCode, cancellationToken);

        return new KataBugFindingSolveResult
        {
            IsScheduled = true
        };
    }
}