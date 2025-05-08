using System.ComponentModel.DataAnnotations;
using GuessCode.DAL.Commands;
using GuessCode.DAL.Contexts;
using GuessCode.DAL.Models.UserAggregate;
using GuessCode.DAL.Models.UserAggregate.Enums;
using GuessCode.Domain.Contracts;
using GuessCode.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace GuessCode.Domain.Services;

public class MentorshipService : IMentorshipService
{
    private readonly GuessContext _context;
    private readonly IConnectionMultiplexer _redis;

    public MentorshipService(GuessContext context, IConnectionMultiplexer redis)
    {
        _context = context;
        _redis = redis;
    }

    public async Task ApplyForMentorship(Mentor mentor, CancellationToken cancellationToken)
    {
        var isAlreadyMentor = await _context
            .Set<Mentor>()
            .AnyAsync(x => x.UserId == mentor.UserId, cancellationToken);

        if (isAlreadyMentor)
        {
            throw new ValidationException($"User {mentor.Id} is already a mentor or applied for mentor");
        }

        mentor.IsApproved = false;
        await _context.AddAsync(mentor, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Mentor>> GetPendingMentors(CancellationToken cancellationToken)
    {
        return await _context
            .Set<Mentor>()
            .AsNoTracking()
            .Where(x => !x.IsApproved)
            .ToListAsync(cancellationToken);
    }

    public async Task ConsiderPendingMentor(long mentorId, bool isApproved, CancellationToken cancellationToken)
    {
        var mentorUserId = await _context
            .Set<Mentor>()
            .AsNoTracking()
            .Where(x => x.Id == mentorId)
            .Select(x => x.UserId)
            .SingleAsync(cancellationToken);
        
        if (isApproved)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            
            await _context
                .Set<Mentor>()
                .Where(x => x.Id == mentorId)
                .ExecuteUpdateAsync(x => x.SetProperty(m => m.IsApproved, true), cancellationToken);
            await CreateApprovedNotification(mentorUserId, cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);
        }
        else
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            await _context
                .Set<Mentor>()
                .Where(x => x.Id == mentorId)
                .ExecuteDeleteAsync(cancellationToken);
            await CreateRejectedNotification(mentorUserId, cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);
        }
    }

    public async Task<List<User>> GetMentees(long userId, CancellationToken cancellationToken)
    {
        return await _context
            .Set<Mentor>()
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .SelectMany(x => x.Mentees)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<User>> GetPendingMentees(long userId, CancellationToken cancellationToken)
    {
        return await _context
            .Set<MentorRequest>()
            .Where(x => x.Mentor.UserId == userId && x.Status == MentorRequestStatus.Sent)
            .Select(x => x.User)
            .ToListAsync(cancellationToken);
    }

    public async Task ConsiderPendingMentee(long userId, long menteeId, bool isApproved, CancellationToken cancellationToken)
    {
        var checkMentorRequestExists = await _context
            .Set<MentorRequest>()
            .AnyAsync(x => x.Mentor.UserId == userId && x.UserId == menteeId, cancellationToken);
        
        var mentorId = await _context
            .Set<Mentor>()
            .Where(x => x.UserId == userId)
            .Select(x => x.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (!checkMentorRequestExists)
        {
            throw new ValidationException($"There is not request for mentor {userId} and mentee {menteeId}");
        }

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        
        var newStatus = isApproved ? MentorRequestStatus.Accepted : MentorRequestStatus.Declined;
        await _context
            .Set<MentorRequest>()
            .Where(x => x.Mentor.UserId == userId && x.UserId == menteeId)
            .ExecuteUpdateAsync(x =>
                x.SetProperty(y => y.Status, newStatus), cancellationToken);

        if (newStatus is MentorRequestStatus.Accepted)
        {
            await _context
                .Set<User>()
                .Where(x => x.Id == menteeId)
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(y => y.MentorId, mentorId), cancellationToken);
        }

        await transaction.CommitAsync(cancellationToken);
    }

    public async Task<Mentor?> GetMentorById(long mentorId, CancellationToken cancellationToken)
    {
        return await _context
            .Set<Mentor>()
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == mentorId, cancellationToken);
    }

    private async Task CreateRejectedNotification(long userId, CancellationToken cancellationToken)
    {
        var (mentorUsername, mentorEmail) = await GetUsernameAndEmailById(userId, cancellationToken);
        
        var emailCommand = new SendRejectedMentorshipEmailCommand()
        {
            ReceiverEmail = mentorEmail,
            Username = mentorUsername
        };

        var redisDatabase = _redis.GetDatabase();

        var existingData = await redisDatabase.StringGetAsync(nameof(SendApprovedMentorshipEmailCommand));
        var currentEmailCommand = string.IsNullOrEmpty(existingData)
            ? new SendRejectedMentorshipEmailCommand[] { }  
            : JsonConvert.DeserializeObject<SendRejectedMentorshipEmailCommand[]>(existingData!);

        var updatedData = JsonConvert.SerializeObject(ArrayUtils.AddToArray(currentEmailCommand!, emailCommand));
        await redisDatabase.StringSetAsync(nameof(SendApprovedMentorshipEmailCommand), updatedData);
    }

    private async Task CreateApprovedNotification(long userId, CancellationToken cancellationToken)
    {
        var (mentorUsername, mentorEmail) = await GetUsernameAndEmailById(userId, cancellationToken);
        
        var emailCommand = new SendApprovedMentorshipEmailCommand()
        {
            ReceiverEmail = mentorEmail,
            Username = mentorUsername
        };

        var redisDatabase = _redis.GetDatabase();

        var existingData = await redisDatabase.StringGetAsync(nameof(SendRejectedMentorshipEmailCommand));
        var currentEmailCommand = string.IsNullOrEmpty(existingData)
            ? new SendApprovedMentorshipEmailCommand[] { }  
            : JsonConvert.DeserializeObject<SendApprovedMentorshipEmailCommand[]>(existingData!);

        var updatedData = JsonConvert.SerializeObject(ArrayUtils.AddToArray(currentEmailCommand!, emailCommand));
        await redisDatabase.StringSetAsync(nameof(SendRejectedMentorshipEmailCommand), updatedData);
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