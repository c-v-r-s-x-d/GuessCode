using GuessCode.DAL.Models.KataAggregate;

namespace GuessCode.Domain.Contracts;

public interface IKataAdministrationService
{
    Task CreateKata(long userId, Kata kata, CancellationToken cancellationToken);

    Task EditKata(long userId, Kata kata, CancellationToken cancellationToken);

    Task DeleteKata(long userId, long kataId, CancellationToken cancellationToken);
}