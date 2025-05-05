using GuessCode.DAL.Models.UserAggregate;

namespace GuessCode.Domain.Contracts;

public interface IMentorshipService
{
    Task ApplyForMentorship(Mentor mentor, CancellationToken cancellationToken);

    Task<List<Mentor>> GetPendingMentors(CancellationToken cancellationToken);
    
    Task ConsiderPendingMentor(long mentorId, bool isApproved, CancellationToken cancellationToken);
}