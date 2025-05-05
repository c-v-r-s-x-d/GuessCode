namespace GuessCode.Domain.AI.Contracts;

public interface IAiKataResolver
{
    Task<string> ResolveKata(string kataDescription, CancellationToken cancellationToken);
}