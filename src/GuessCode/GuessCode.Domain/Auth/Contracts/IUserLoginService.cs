using GuessCode.Domain.Auth.Models;

namespace GuessCode.Domain.Auth.Contracts;

public interface IUserLoginService
{
    Task<Token> GetAccessToken(string username, string password, string secretKey, string issuer, string audience,
        CancellationToken cancellationToken);
}