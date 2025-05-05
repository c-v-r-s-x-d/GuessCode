using System.ComponentModel.DataAnnotations;
using GuessCode.DAL.Commands;
using GuessCode.DAL.Contexts;
using GuessCode.DAL.Models.KataAggregate;
using GuessCode.DAL.Models.UserAggregate;
using GuessCode.Domain.Contracts;
using GuessCode.Domain.File.Contracts;
using GuessCode.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace GuessCode.Domain.Services;

public class KataAdministrationService : IKataAdministrationService
{
    private const string TestFileExtension = ".txt";
    
    private readonly GuessContext _context;
    private readonly IFileUploaderService _fileUploaderService;
    private readonly IConnectionMultiplexer _redis;

    public KataAdministrationService(GuessContext context, IFileUploaderService fileUploaderService, IConnectionMultiplexer redis)
    {
        _context = context;
        _fileUploaderService = fileUploaderService;
        _redis = redis;
    }

    public async Task ConsiderPendingKata(long kataId, bool isApproved, CancellationToken cancellationToken)
    {
        var authorUserId = await _context
            .Set<Kata>()
            .AsNoTracking()
            .Where(x => x.Id == kataId)
            .Select(x => x.AuthorId)
            .SingleAsync(cancellationToken);
        
        if (isApproved)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            
            await _context
                .Set<Kata>()
                .Where(x => x.Id == kataId)
                .ExecuteUpdateAsync(x => x.SetProperty(m => m.IsApproved, true), cancellationToken);
            await CreateApprovedNotification(authorUserId, cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);
        }
        else
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            await _context
                .Set<Kata>()
                .Where(x => x.Id == kataId)
                .ExecuteDeleteAsync(cancellationToken);
            await CreateRejectedNotification(authorUserId, cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);
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

    public async Task ApplyKataForCreation(long userId, Kata kata, byte[]? testFile, CancellationToken cancellationToken)
    {
        EnsureUserIsAuthor(userId, kata.AuthorId);

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        Guid? fileId = null;
        if (testFile is not null)
        {
            fileId = await _fileUploaderService.UploadFile(testFile, TestFileExtension, false, cancellationToken);
        }

        kata.IsApproved = false;
        
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
        
        await transaction.CommitAsync(cancellationToken);
    }

    public async Task<List<Kata>> GetPendingKatas(CancellationToken cancellationToken)
    {
        return await _context
            .Set<Kata>()
            .AsNoTracking()
            .Where(x => !x.IsApproved)
            .ToListAsync(cancellationToken);
    }

    private static void EnsureUserIsAuthor(long userId, long authorId)
    {
        if (userId != authorId)
        {
            throw new ValidationException("Kata administration not in the role of the author is prohibited");
        }
    }
    
    private async Task CreateRejectedNotification(long userId, CancellationToken cancellationToken)
    {
        var (mentorUsername, mentorEmail) = await GetUsernameAndEmailById(userId, cancellationToken);
        
        var emailCommand = new SendApprovedKataEmailCommand()
        {
            ReceiverEmail = mentorEmail,
            Username = mentorUsername
        };

        var redisDatabase = _redis.GetDatabase();

        var existingData = await redisDatabase.StringGetAsync(nameof(SendApprovedKataEmailCommand));
        var currentEmailCommand = string.IsNullOrEmpty(existingData)
            ? new SendApprovedKataEmailCommand[] { }  
            : JsonConvert.DeserializeObject<SendApprovedKataEmailCommand[]>(existingData!);

        var updatedData = JsonConvert.SerializeObject(ArrayUtils.AddToArray(currentEmailCommand!, emailCommand));
        await redisDatabase.StringSetAsync(nameof(SendApprovedKataEmailCommand), updatedData);
    }

    private async Task CreateApprovedNotification(long userId, CancellationToken cancellationToken)
    {
        var (mentorUsername, mentorEmail) = await GetUsernameAndEmailById(userId, cancellationToken);
        
        var emailCommand = new SendRejectedKataEmailCommand()
        {
            ReceiverEmail = mentorEmail,
            Username = mentorUsername
        };

        var redisDatabase = _redis.GetDatabase();

        var existingData = await redisDatabase.StringGetAsync(nameof(SendRejectedKataEmailCommand));
        var currentEmailCommand = string.IsNullOrEmpty(existingData)
            ? new SendRejectedKataEmailCommand[] { }  
            : JsonConvert.DeserializeObject<SendRejectedKataEmailCommand[]>(existingData!);

        var updatedData = JsonConvert.SerializeObject(ArrayUtils.AddToArray(currentEmailCommand!, emailCommand));
        await redisDatabase.StringSetAsync(nameof(SendRejectedKataEmailCommand), updatedData);
    }

    private async Task<(string Username, string Email)> GetUsernameAndEmailById(long userId, CancellationToken cancellationToken)
    {
        var result = await _context
            .Set<User>()
            .AsNoTracking()
            .Where(x => x.Id == userId)
            .Select(x => new
            {
                x.Username,
                x.Email
            })
            .SingleAsync(cancellationToken);

        return (result.Username, result.Email);
    }
}