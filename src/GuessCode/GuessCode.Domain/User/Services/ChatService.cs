using System.ComponentModel.DataAnnotations;
using GuessCode.DAL.Contexts;
using GuessCode.DAL.Models.UserAggregate;
using GuessCode.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GuessCode.Domain.Services;

public class ChatService : IChatService
{
    private readonly GuessContext _context;

    public ChatService(GuessContext context)
    {
        _context = context;
    }

    public async Task<Guid> GetOrCreateChatRoom(long firstUserId, long secondUserId,
        CancellationToken cancellationToken)
    {
        if (firstUserId == secondUserId)
        {
            throw new ValidationException("Chat with yourself not supported");
        }

        var user1Id = Math.Min(firstUserId, secondUserId);
        var user2Id = Math.Max(firstUserId, secondUserId);

        var room = await _context
            .Set<ChatRoom>()
            .SingleOrDefaultAsync(x => x.User1Id == user1Id && x.User2Id == user2Id, cancellationToken);

        if (room is null)
        {
            room = new ChatRoom
            {
                User1Id = user1Id,
                User2Id = user2Id,
                RoomId = Guid.NewGuid()
            };
            await _context.AddAsync(room, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return room.RoomId;
    }

    public async Task EnsureUserIsEligibleToJoinRoom(long userId, Guid roomId, CancellationToken cancellationToken)
    {
        var isEligible = await _context
            .Set<ChatRoom>()
            .AnyAsync(x => x.RoomId == roomId && x.User1Id == userId || x.User2Id == userId, cancellationToken);

        if (!isEligible)
        {
            throw new ValidationException("You are not allowed to join the room");
        }
    }
}
