using AutoMapper;
using GuessCode.API.Models.V1.Kata;
using GuessCode.DAL.Models.RoleAggregate;
using GuessCode.Domain.Contracts;
using GuessCode.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GuessCode.API.Controllers;

[ApiController]
[Route("api/kata-solve")]
[Authorize(Roles = $"{RoleNameConstants.Admin}, {RoleNameConstants.User}")]
public class KataSolveController : BaseGuessController
{
    private readonly IMapper _mapper;
    private readonly IKataCodeReadingSolveService _kataCodeReadingSolveService;
    private readonly IKataBugFindingSolveService _kataBugFindingSolveService;

    public KataSolveController(IKataCodeReadingSolveService kataCodeReadingSolveService, IMapper mapper, IKataBugFindingSolveService kataBugFindingSolveService)
    {
        _kataCodeReadingSolveService = kataCodeReadingSolveService;
        _mapper = mapper;
        _kataBugFindingSolveService = kataBugFindingSolveService;
    }

    [HttpPut("code-reading")]
    public async Task<KataCodeReadingSolveResultDto> SolveCodeReadingKata([FromBody] KataCodeReadingAnswerDto kataCodeReadingAnswerDto, CancellationToken cancellationToken)
    {
        var kataAnswer = _mapper.Map<KataCodeReadingAnswer>(kataCodeReadingAnswerDto);
        return _mapper.Map<KataCodeReadingSolveResultDto>(
            await _kataCodeReadingSolveService.SolveKata(UserId, kataAnswer, cancellationToken));
    }

    [HttpPut("bug-finding")]
    public async Task<KataBugFindingSolveResultDto> SolveBugFindingKata(
        [FromBody] KataBugFindingAnswerDto kataBugFindingAnswerDto, CancellationToken cancellationToken)
    {
        var kataAnswer = _mapper.Map<KataBugFindingAnswer>(kataBugFindingAnswerDto);
        return _mapper.Map<KataBugFindingSolveResultDto>(
            await _kataBugFindingSolveService.SolveKata(UserId, kataAnswer, cancellationToken));
    }
}