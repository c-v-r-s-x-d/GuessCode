using GuessCode.DAL.Models.UserAggregate;

namespace GuessCode.Domain.Contracts;

public interface IUserService
{
    Task<List<User>> GetAllUsers(CancellationToken cancellationToken);
    
    Task<User> GetUserById(long userId, CancellationToken cancellationToken);
}