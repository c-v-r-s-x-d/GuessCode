using GuessCode.Domain.Models;

namespace GuessCode.Domain.Contracts;

public interface IKataBugFindingSolveService
{
    Task<KataBugFindingSolveResult> SolveKata(long userId, KataBugFindingAnswer kataCodeReadingAnswer, CancellationToken cancellationToken);
}