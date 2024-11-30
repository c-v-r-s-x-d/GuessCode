using GuessCode.DAL.Models.UserAggregate;

namespace GuessCode.Domain.Contracts;

public interface IUserService
{
    Task<User> GetUserById(long userId, CancellationToken cancellationToken);
}