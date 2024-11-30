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
    private readonly IKataSolveService _kataSolveService;

    public KataSolveController(IKataSolveService kataSolveService, IMapper mapper)
    {
        _kataSolveService = kataSolveService;
        _mapper = mapper;
    }

    [HttpPut]
    public async Task<KataSolveResultDto> SolveKata([FromBody] KataAnswerDto kataAnswerDto, CancellationToken cancellationToken)
    {
        var kataAnswer = _mapper.Map<KataAnswer>(kataAnswerDto);
        return _mapper.Map<KataSolveResultDto>(await _kataSolveService.SolveKata(kataAnswer, cancellationToken));
    }
}