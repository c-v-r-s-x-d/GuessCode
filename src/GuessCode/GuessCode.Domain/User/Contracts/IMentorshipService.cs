using GuessCode.DAL.Models.UserAggregate;

namespace GuessCode.Domain.Contracts;

public interface IMentorshipService
{
    Task ApplyForMentorship(Mentor mentor, CancellationToken cancellationToken);

    Task<List<Mentor>> GetPendingMentors(CancellationToken cancellationToken);
    
    Task ConsiderPendingMentor(long mentorId, bool isApproved, CancellationToken cancellationToken);
    
    Task<List<User>> GetMentees(long userId, CancellationToken cancellationToken);
    
    Task<List<User>> GetPendingMentees(long userId, CancellationToken cancellationToken);
    
    Task ConsiderPendingMentee(long userId, long menteeId, bool isApproved, CancellationToken cancellationToken);
    
    Task<Mentor?> GetMentorById(long mentorId, CancellationToken cancellationToken);
}