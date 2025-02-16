using GuessCode.Domain.Models;

namespace GuessCode.Domain.Contracts;

public interface IKataSolveService
{
    Task<KataSolveResult> SolveKata(long userId, KataAnswer kataAnswer, CancellationToken cancellationToken);
}