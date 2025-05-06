using System.Collections.Concurrent;
using GuessCode.Domain.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace GuessCode.Domain.Hubs;

public class ChatHub : Hub
{
    private static readonly ConcurrentDictionary<Guid, List<(long userId, string message)>> ChatHistory = new();
    
    private readonly IChatService _chatService;

    public ChatHub(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task JoinRoom(Guid roomName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomName.ToString());
        
        if (ChatHistory.TryGetValue(roomName, out var messages))
        {
            foreach (var (userId, message) in messages)
            {
                await _chatService.EnsureUserIsEligibleToJoinRoom(userId, roomName, CancellationToken.None);
                await Clients.Caller.SendAsync("ReceiveMessage", userId, message);
            }
        }
    }

    public async Task LeaveRoom(Guid roomName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName.ToString());
    }

    public async Task SendMessageToRoom(Guid roomName, long userId, string message)
    {
        ChatHistory.AddOrUpdate(roomName,
            _ => new List<(long, string)> { (userId, message) },
            (_, list) =>
            {
                list.Add((userId, message));
                return list;
            });

        // Рассылаем всем в комнате
        await Clients.Group(roomName.ToString()).SendAsync("ReceiveMessage", userId, message);
    }
}