using System.ComponentModel.DataAnnotations;
using GuessCode.DAL.Contexts;
using GuessCode.DAL.Models.KataAggregate;
using GuessCode.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GuessCode.Domain.Services;

public class KataAdministrationService : IKataAdministrationService
{
    private readonly GuessContext _context;

    public KataAdministrationService(GuessContext context)
    {
        _context = context;
    }

    public async Task CreateKata(long userId, Kata kata, CancellationToken cancellationToken)
    {
        EnsureUserIsAuthor(userId, kata.AuthorId);

        await _context.AddAsync(kata, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task EditKata(long userId, Kata kata, CancellationToken cancellationToken)
    {
        EnsureUserIsAuthor(userId, kata.AuthorId);
        
        _context.Update(kata);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteKata(long userId, long kataId, CancellationToken cancellationToken)
    {
        var kata = (await _context
                       .Set<Kata>()
                       .Where(x => x.Id == kataId)
                       .SingleOrDefaultAsync(x => x.Id == kataId, cancellationToken)) ??
                   throw new ValidationException($"Kata {kataId} not found");
        EnsureUserIsAuthor(userId, kata.AuthorId);

        _context.Remove(kata);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static void EnsureUserIsAuthor(long userId, long authorId)
    {
        if (userId != authorId)
        {
            throw new ValidationException("Kata administration not in the role of the author is prohibited");
        }
    }
}