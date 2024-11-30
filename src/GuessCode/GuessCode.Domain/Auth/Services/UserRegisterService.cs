using System.ComponentModel.DataAnnotations;
using GuessCode.DAL.Commands;
using GuessCode.DAL.Contexts;
using GuessCode.DAL.Models;
using GuessCode.DAL.Models.Enums;
using GuessCode.DAL.Models.UserAggregate;
using GuessCode.Domain.Auth.Contracts;
using GuessCode.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackExchange.Redis;
using Role = GuessCode.DAL.Models.RoleAggregate.Role;

namespace GuessCode.Domain.Auth.Services;

public class UserRegisterService : IUserRegisterService
{
    private readonly GuessContext _context;
    private readonly IConnectionMultiplexer _redis;

    public UserRegisterService(GuessContext context, IConnectionMultiplexer redis)
    {
        _context = context;
        _redis = redis;
    }

    public async Task Register(string email, string username, string password, CancellationToken cancellationToken)
    {
        EnsureValidRegistrationData(email, password);
        await EnsureUserDoesNotExist(username, cancellationToken);

        await CreateUser(email, username, password, cancellationToken);
        await CreateWelcomeNotification(username, email);
    }

    private static void EnsureValidRegistrationData(string email, string password)
    {
        if (!RegexUtils.IsValidEmail(email))
        {
            throw new ValidationException("Wrong email format");
        }

        if (!RegexUtils.IsValidPassword(password))
        {
            throw new ValidationException("Wrong password format");
        }
    }

    private async Task EnsureUserDoesNotExist(string username, CancellationToken cancellationToken)
    {
        var userExists = await _context
            .Set<User>()
            .AnyAsync(x => x.Username == username, cancellationToken);

        if (userExists)
        {
            throw new ValidationException("This username already taken");
        }
    }

    private async Task CreateUser(string email, string username, string password, CancellationToken cancellationToken)
    {
        var userRole = await GetDefaultUserRole();

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        
        var user = new User
        {
            Username = username,
            Password = PasswordUtils.GetHash(password),
            Email = email,
            RegistrationDate = DateTime.UtcNow,
            Rank = Rank.FifthKyu,
            RoleId = userRole?.Id
        };
        await _context.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        var userProfile = new UserProfile
        {
            Description = "Hello, world! I'm new here",
            ActivityStatus = ActivityStatus.Online,
            UserId = user.Id
        };
        await _context.AddAsync(userProfile, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }

    private Task<Role?> GetDefaultUserRole()
    {
        return _context.Set<Role>().SingleOrDefaultAsync(x => x.Name == "User");
    }

    private async Task CreateWelcomeNotification(string username, string email)
    {
        var emailCommand = new SendWelcomeEmailCommand
        {
            ReceiverEmail = email,
            Username = username
        };

        var redisDatabase = _redis.GetDatabase();

        var existingData = await redisDatabase.StringGetAsync(nameof(SendWelcomeEmailCommand));
        var currentEmailCommand = string.IsNullOrEmpty(existingData)
            ? new SendWelcomeEmailCommand[] { }  
            : JsonConvert.DeserializeObject<SendWelcomeEmailCommand[]>(existingData!);

        var updatedData = JsonConvert.SerializeObject(ArrayUtils.AddToArray(currentEmailCommand!, emailCommand));
        await redisDatabase.StringSetAsync(nameof(SendWelcomeEmailCommand), updatedData);
    }
}