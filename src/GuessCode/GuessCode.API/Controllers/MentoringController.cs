using AutoMapper;
using GuessCode.API.Models.V1.User;
using GuessCode.DAL.Models.Enums;
using GuessCode.DAL.Models.RoleAggregate;
using GuessCode.DAL.Models.UserAggregate;
using GuessCode.Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GuessCode.API.Controllers;

[ApiController]
[Authorize(Roles = $"{RoleNameConstants.Admin}, {RoleNameConstants.User}")]
[Route("api/mentoring")]
public class MentoringController : BaseGuessController
{
    private readonly IMentorshipService _mentorshipService;
    private readonly IFindMentorService _findMentorService;
    
    private readonly IMapper _mapper;

    public MentoringController(IMentorshipService mentorshipService, IFindMentorService findMentorService, IMapper mapper)
    {
        _mentorshipService = mentorshipService;
        _findMentorService = findMentorService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<List<MentorDto>> GetMentorsByFilter(List<ProgrammingLanguage> programmingLanguages, CancellationToken cancellationToken)
    {
        return _mapper.Map<List<MentorDto>>(
            await _findMentorService.GetMentorsByFilter(programmingLanguages, cancellationToken));
    }

    [HttpPost("mentorship")]
    public async Task ApplyForMentorship([FromBody] MentorDto mentorDto, CancellationToken cancellationToken)
    {
        await _mentorshipService.ApplyForMentorship(_mapper.Map<Mentor>(mentorDto), cancellationToken);
    }

    [HttpPost("mentor-request")]
    public async Task RequestForMentor(long mentorId, CancellationToken cancellationToken)
    {
        await _findMentorService.RequestForMentor(UserId, mentorId, cancellationToken);
    }

    [HttpGet("pending-mentors")]
    [Authorize(Roles = $"{RoleNameConstants.Admin}")]
    public async Task<List<MentorDto>> GetPendingMentors(CancellationToken cancellationToken)
    {
        return _mapper.Map<List<MentorDto>>(await _mentorshipService.GetPendingMentors(cancellationToken));
    }

    [HttpPost("pending-mentors")]
    [Authorize(Roles = $"{RoleNameConstants.Admin}")]
    public async Task ConsiderPendingMentor(long mentorId, bool isApproved, CancellationToken cancellationToken)
    {
        await _mentorshipService.ConsiderPendingMentor(mentorId, isApproved, cancellationToken);
    }
}