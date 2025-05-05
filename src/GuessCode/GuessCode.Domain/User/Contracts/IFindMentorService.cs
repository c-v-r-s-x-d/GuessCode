using GuessCode.DAL.Models.Enums;
using GuessCode.DAL.Models.UserAggregate;

namespace GuessCode.Domain.Contracts;

public interface IFindMentorService
{
    Task<List<Mentor>> GetMentorsByFilter(List<ProgrammingLanguage> programmingLanguages, CancellationToken cancellationToken);

    Task RequestForMentor(long userId, long mentorId, CancellationToken cancellationToken);
}