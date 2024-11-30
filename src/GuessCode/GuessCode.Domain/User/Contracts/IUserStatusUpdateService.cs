namespace GuessCode.Domain.Contracts;

public interface IUserStatusUpdateService
{
    Task HandleUserActivityStatusOffline(long userId, CancellationToken cancellationToken);

    Task HandleUserActivityStatusOnline(long userId, CancellationToken cancellationToken);
}