using GuessCode.Domain.Models;

namespace GuessCode.Domain.Contracts;

public interface IKataCodeReviewSolveService
{
    Task<KataCodeReviewSolveResult> SolveKata(long userId, KataCodeReviewAnswer kataCodeReadingAnswer, CancellationToken cancellationToken);
}