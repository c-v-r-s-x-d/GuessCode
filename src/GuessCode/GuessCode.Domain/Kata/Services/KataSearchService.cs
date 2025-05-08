using System.ComponentModel.DataAnnotations;
using GuessCode.DAL.Contexts;
using GuessCode.DAL.Models.Enums;
using GuessCode.DAL.Models.KataAggregate;
using GuessCode.DAL.Models.UserAggregate;
using GuessCode.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GuessCode.Domain.Services;

public class KataSearchService : IKataSearchService
{
    private readonly GuessContext _context;

    public KataSearchService(GuessContext context)
    {
        _context = context;
    }

    public async Task<Kata> GetKataById(long kataId, CancellationToken cancellationToken)
    {
        return (await _context
                   .Set<Kata>()
                   .SingleOrDefaultAsync(x => x.Id == kataId, cancellationToken)) ??
                    throw new ValidationException($"Kata {kataId} not found");
    }

    public async Task<IReadOnlyCollection<Kata>> Search(KataDifficulty? kataDifficulty, ProgrammingLanguage? kataLanguage, KataType? kataType,
        CancellationToken cancellationToken)
    {
        var query = _context.Set<Kata>().AsNoTracking().Where(x => x.IsApproved);

        if (kataDifficulty is not null)
        {
            query = query.Where(x => x.KataDifficulty == kataDifficulty.Value);
        }

        if (kataLanguage is not null)
        {
            query = query.Where(x => x.ProgrammingLanguage == kataLanguage.Value);
        }

        if (kataType is not null)
        {
            query = query.Where(x => x.KataType == kataType.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<long>> GetUserResolvedKataIds(long userId, CancellationToken cancellationToken)
    {
        return await _context
            .Set<User>()
            .AsNoTracking()
            .Where(x => x.Id == userId)
            .SelectMany(x => x.ResolvedKatas)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<KataCodeExecutionResult?> GetResolvedKataCodeExecutionResult(long userId, long kataId, CancellationToken cancellationToken)
    {
        return await _context
            .Set<KataCodeExecutionResult>()
            .Where(x => x.KataId == kataId && x.ExecutedBy == userId)
            .OrderByDescending(x => x.ExecutedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }
}