using System.ComponentModel.DataAnnotations;
using GuessCode.DAL.Contexts;
using GuessCode.DAL.Models.KataAggregate;
using GuessCode.Domain.Contracts;
using GuessCode.Domain.File.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GuessCode.Domain.Services;

public class KataAdministrationService : IKataAdministrationService
{
    private const string TestFileExtension = ".txt";
    
    private readonly GuessContext _context;
    private readonly IFileUploaderService _fileUploaderService;

    public KataAdministrationService(GuessContext context, IFileUploaderService fileUploaderService)
    {
        _context = context;
        _fileUploaderService = fileUploaderService;
    }

    public async Task CreateKata(long userId, Kata kata, byte[]? testFile, CancellationToken cancellationToken)
    {
        EnsureUserIsAuthor(userId, kata.AuthorId);

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        Guid? fileId = null;
        if (testFile is not null)
        {
            fileId = await _fileUploaderService.UploadFile(testFile, TestFileExtension, false, cancellationToken);
        }

        await _context.AddAsync(kata, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        if (fileId is not null)
        {
            await _context.AddAsync(new KataTestFile
            {
                KataId = kata.Id,
                FileName = fileId.Value.ToString()
            }, cancellationToken);
        }
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