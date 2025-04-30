using GuessCode.Domain.Models;

namespace GuessCode.Domain.Contracts;

public interface IProfileInfoService
{
    Task<ProfileInfo> GetUserProfileInfo(long userId, CancellationToken cancellationToken);
    
    Task UpdateAvatar(long userId, byte[] avatar, string fileExtension, CancellationToken cancellationToken);
}