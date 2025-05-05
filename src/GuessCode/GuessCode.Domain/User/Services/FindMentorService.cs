using System.ComponentModel.DataAnnotations;
using GuessCode.DAL.Contexts;
using GuessCode.DAL.Models.Enums;
using GuessCode.DAL.Models.UserAggregate;
using GuessCode.DAL.Models.UserAggregate.Enums;
using GuessCode.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GuessCode.Domain.Services;

public class FindMentorService : IFindMentorService
{
    private readonly GuessContext _context;

    public FindMentorService(GuessContext context)
    {
        _context = context;
    }

    public async Task<List<Mentor>> GetMentorsByFilter(List<ProgrammingLanguage> programmingLanguages, CancellationToken cancellationToken)
    {
        return await _context
            .Set<Mentor>()
            .Where(x => x.ProgrammingLanguages.Any(programmingLanguages.Contains) && x.IsApproved)
            .ToListAsync(cancellationToken);
    }

    public async Task RequestForMentor(long userId, long mentorId, CancellationToken cancellationToken)
    {
        var isRequestExits = await _context
            .Set<MentorRequest>()
            .Where(x => x.UserId == userId && x.MentorId == mentorId)
            .AnyAsync(cancellationToken);

        if (isRequestExits)
        {
            throw new ValidationException("Mentor request is already exits");
        }

        await _context.AddAsync(new MentorRequest
        {
            MentorId = mentorId,
            UserId = userId,
            Status = MentorRequestStatus.Sent
        }, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}