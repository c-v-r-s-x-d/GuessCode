using System.ComponentModel.DataAnnotations;
using AutoMapper;
using GuessCode.API.Models.V1.User;
using GuessCode.DAL.Models.RoleAggregate;
using GuessCode.Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GuessCode.API.Controllers;

[Authorize(Roles = $"{RoleNameConstants.Admin}, {RoleNameConstants.User}")]
[ApiController]
[Route("api/profile-info")]
public class ProfileInfoController : BaseGuessController
{
    private readonly IMapper _mapper;
    private readonly IProfileInfoService _profileInfoService;

    public ProfileInfoController(IMapper mapper, IProfileInfoService profileInfoService)
    {
        _mapper = mapper;
        _profileInfoService = profileInfoService;
    }

    [HttpGet]
    [Route("{userId}")]
    public async Task<ProfileInfoDto> GetProfileInfo(long userId, CancellationToken cancellationToken)
    {
        return _mapper.Map<ProfileInfoDto>(await _profileInfoService.GetUserProfileInfo(userId, cancellationToken));
    }

    [HttpPost]
    [Route("{userId}")]
    public async Task UpdateAvatar([FromQuery] long userId, [FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        if (userId != UserId)
        {
            throw new ValidationException("Permission denied");
        }
        
        if (file is null || file.Length == 0)
        {
            throw new ValidationException("File is empty");
        }
        
        using var memoryStream = new MemoryStream();
        var fileExtension = Path.GetExtension(file.FileName);
        if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png")
        {
            throw new ValidationException("File format is not supported");
        }
        
        await file.CopyToAsync(memoryStream, cancellationToken);
        var fileBytes = memoryStream.ToArray();
        
        await _profileInfoService.UpdateAvatar(userId, fileBytes, fileExtension, cancellationToken);
    }
}