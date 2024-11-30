using System.ComponentModel.DataAnnotations;
using GuessCode.Domain.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace GuessCode.Domain.Hubs;

public class UserStatusHub : Hub
{
    private readonly IUserStatusUpdateService _statusUpdateService;

    public UserStatusHub(IUserStatusUpdateService statusUpdateService)
    {
        _statusUpdateService = statusUpdateService;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = ExtractUserIdFromQuery();
        await _statusUpdateService
            .HandleUserActivityStatusOnline(Convert.ToInt64(userId), default);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userId = ExtractUserIdFromQuery();
        await _statusUpdateService
            .HandleUserActivityStatusOffline(Convert.ToInt64(userId), default);

        await base.OnDisconnectedAsync(exception);
    }

    private long ExtractUserIdFromQuery()
    {
        var httpContext = Context.GetHttpContext();
        var userId = httpContext.Request.Query["userId"];

        if (!long.TryParse(userId, out var result))
        {
            throw new ValidationException("Failed to extract ID");
        }

        return result;
    }
}