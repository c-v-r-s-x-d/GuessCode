using GuessCode.Domain.Models;

namespace GuessCode.Domain.Contracts;

public interface IKataSolveService
{
    Task<KataSolveResult> SolveKata(KataAnswer kataAnswer, CancellationToken cancellationToken);
}