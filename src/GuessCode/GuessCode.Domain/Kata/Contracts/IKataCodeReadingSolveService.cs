using GuessCode.Domain.Models;

namespace GuessCode.Domain.Contracts;

public interface IKataCodeReadingSolveService
{
    Task<KataCodeReadingSolveResult> SolveKata(long userId, KataCodeReadingAnswer kataCodeReadingAnswer, CancellationToken cancellationToken);
}