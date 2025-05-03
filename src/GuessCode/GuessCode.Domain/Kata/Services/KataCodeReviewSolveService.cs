using GuessCode.Domain.Contracts;
using GuessCode.Domain.Models;

namespace GuessCode.Domain.Services;

public class KataCodeReviewSolveService : IKataCodeReviewSolveService
{
    public Task<KataCodeReviewSolveResult> SolveKata(long userId, KataCodeReviewAnswer kataCodeReadingAnswer, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}