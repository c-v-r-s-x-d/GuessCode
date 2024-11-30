using GuessCode.Domain.Auth.Models;

namespace GuessCode.Domain.Auth.Contracts;

public interface IGitHubLoginService
{
    Task<Token> Login(string code, CancellationToken cancellationToken);
}