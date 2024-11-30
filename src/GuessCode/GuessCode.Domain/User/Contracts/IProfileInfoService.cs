using GuessCode.Domain.Models;

namespace GuessCode.Domain.Contracts;

public interface IProfileInfoService
{
    Task<ProfileInfo> GetUserProfileInfo(long userId, CancellationToken cancellationToken);
}