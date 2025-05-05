using AutoMapper;
using GuessCode.API.Models.V1.User;
using GuessCode.DAL.Models.RoleAggregate;
using GuessCode.Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GuessCode.API.Controllers;

[ApiController]
[Route("api/user")]
[Authorize(Roles = $"{RoleNameConstants.Admin}, {RoleNameConstants.User}")]
public class UserController : BaseGuessController
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public UserController(IMapper mapper, IUserService userService)
    {
        _mapper = mapper;
        _userService = userService;
    }

    [HttpGet]
    public async Task<UserDto> GetUserById([FromQuery] long userId, CancellationToken cancellationToken)
    {
        return _mapper.Map<UserDto>(await _userService.GetUserById(userId, cancellationToken));
    }

    [HttpGet("all")]
    [Authorize(Roles = $"{RoleNameConstants.Admin}")]
    public async Task<List<UserDto>> GetAllUsers(CancellationToken cancellationToken)
    {
        return _mapper.Map<List<UserDto>>(await _userService.GetAllUsers(cancellationToken));
    }
    
    
}