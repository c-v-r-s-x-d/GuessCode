using GuessCode.DAL.Models.RoleAggregate;
using GuessCode.Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GuessCode.API.Controllers;

[ApiController]
[Authorize(Roles = $"{RoleNameConstants.Admin}, {RoleNameConstants.User}")]
[Route("api/chat")]
public class ChatController : BaseGuessController
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpGet]
    public async Task<Guid> GetOrCreateChatRoom(long secondUserId, CancellationToken cancellationToken)
    {
        return await _chatService.GetOrCreateChatRoom(UserId, secondUserId, cancellationToken);
    }
}