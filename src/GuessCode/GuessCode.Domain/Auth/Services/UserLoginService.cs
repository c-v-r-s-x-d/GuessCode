using System.ComponentModel.DataAnnotations;
using GuessCode.DAL.Contexts;
using GuessCode.DAL.Models.UserAggregate;
using GuessCode.Domain.Auth.Contracts;
using GuessCode.Domain.Auth.Models;
using GuessCode.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GuessCode.Domain.Auth.Services;

public class UserLoginService : IUserLoginService
{
    private readonly ILogger<IUserLoginService> _logger;
    private readonly GuessContext _context;

    public UserLoginService(GuessContext context, ILogger<IUserLoginService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Token> GetAccessToken(string username, string password, string secretKey, string issuer,
        string audience, CancellationToken cancellationToken)
    {
        var requestedUser = await _context
                                .Set<User>()
                                .Include(x => x.Role)
                                .SingleOrDefaultAsync(x => x.Username == username, cancellationToken)
                            ?? throw new ValidationException("User not found");

        if (!PasswordUtils.VerifyPassword(requestedUser.Password, password))
        {
            throw new ValidationException("Wrong username or password");
        }

        var token = new Token
        {
            AccessToken = TokenUtils.GenerateToken(secretKey, issuer, audience, requestedUser.Username,
                requestedUser.Role?.Name),
            UserId = requestedUser.Id
        };
        
        _logger.LogInformation($"User {requestedUser.Id} is authenticated");

        return token;
    }
}