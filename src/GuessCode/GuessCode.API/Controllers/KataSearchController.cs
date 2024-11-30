using AutoMapper;
using GuessCode.API.Models.V1.Kata;
using GuessCode.DAL.Models.Enums;
using GuessCode.DAL.Models.RoleAggregate;
using GuessCode.Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GuessCode.API.Controllers;

[ApiController]
[Route("api/kata-search")]
[Authorize(Roles = $"{RoleNameConstants.Admin}, {RoleNameConstants.User}")]
public class KataSearchController : BaseGuessController
{
    private readonly IMapper _mapper;
    private readonly IKataSearchService _kataSearchService;

    public KataSearchController(IMapper mapper, IKataSearchService kataSearchService)
    {
        _mapper = mapper;
        _kataSearchService = kataSearchService;
    }

    [HttpGet("{kataId}")]
    public async Task<KataDto> GetKataById(long kataId, CancellationToken cancellationToken)
    {
        return _mapper.Map<KataDto>(await _kataSearchService.GetKataById(kataId, cancellationToken));
    }

    [HttpGet]
    public async Task<IReadOnlyCollection<KataDto>> KataSearchEngine([FromQuery] ProgrammingLanguage? kataLanguage,
        [FromQuery] KataType? kataType, [FromQuery] KataDifficulty? kataDifficulty, CancellationToken cancellationToken)
    {
        return _mapper.Map<IReadOnlyCollection<KataDto>>(await _kataSearchService
            .Search(kataDifficulty, kataLanguage, kataType, cancellationToken));
    }
}