namespace GuessCode.Domain.Contracts;

public interface IChatService
{
    Task<Guid> GetOrCreateChatRoom(long firstUserId, long secondUserId, CancellationToken cancellationToken);
    
    Task EnsureUserIsEligibleToJoinRoom(long userId, Guid roomId, CancellationToken cancellationToken);
}