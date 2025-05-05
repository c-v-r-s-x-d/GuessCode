using System.ComponentModel.DataAnnotations;
using GuessCode.DAL.Contexts;
using GuessCode.DAL.Models.UserAggregate;
using GuessCode.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GuessCode.Domain.Services;

public class UserService : IUserService
{
    private readonly GuessContext _context;

    public UserService(GuessContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllUsers(CancellationToken cancellationToken)
    {
        return await _context
            .Set<User>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<User> GetUserById(long userId, CancellationToken cancellationToken)
    {
        return (await _context
                   .Set<User>()
                   .AsNoTracking()
                   .SingleOrDefaultAsync(x => x.Id == userId, cancellationToken)) ??
               throw new ValidationException($"User {userId} not found");
    }
}