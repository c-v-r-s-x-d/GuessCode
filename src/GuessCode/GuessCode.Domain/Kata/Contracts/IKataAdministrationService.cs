using GuessCode.DAL.Models.KataAggregate;

namespace GuessCode.Domain.Contracts;

public interface IKataAdministrationService
{
    Task ConsiderPendingKata(long kataId, bool isApproved, CancellationToken cancellationToken);

    Task EditKata(long userId, Kata kata, CancellationToken cancellationToken);

    Task DeleteKata(long userId, long kataId, CancellationToken cancellationToken);
    
    Task ApplyKataForCreation(long userId, Kata kata, byte[]? testFile, CancellationToken cancellationToken);

    Task<List<Kata>> GetPendingKatas(CancellationToken cancellationToken);
}