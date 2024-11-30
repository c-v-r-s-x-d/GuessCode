namespace GuessCode.Domain.Auth.Contracts;

public interface IUserRegisterService
{
    Task Register(string email, string username, string password, CancellationToken cancellationToken);
}