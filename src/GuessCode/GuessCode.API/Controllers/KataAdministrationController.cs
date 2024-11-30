using AutoMapper;
using GuessCode.API.Models.V1.Kata;
using GuessCode.DAL.Models.KataAggregate;
using GuessCode.DAL.Models.RoleAggregate;
using GuessCode.Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GuessCode.API.Controllers;

[ApiController]
[Authorize(Roles = $"{RoleNameConstants.Admin}")]
[Route("api/kata-administration")]
public class KataAdministrationController : BaseGuessController
{
    private readonly IMapper _mapper;
    private readonly IKataAdministrationService _kataAdministrationService;

    public KataAdministrationController(IKataAdministrationService kataAdministrationService, IMapper mapper)
    {
        _kataAdministrationService = kataAdministrationService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task CreateKata([FromBody] KataDto kataDto, CancellationToken cancellationToken)
    {
        var kata = _mapper.Map<Kata>(kataDto);
        await _kataAdministrationService.CreateKata(UserId, kata, cancellationToken);
    }

    [HttpPut]
    public async Task EditKata([FromBody] KataDto kataDto, CancellationToken cancellationToken)
    {
        var kata = _mapper.Map<Kata>(kataDto);
        await _kataAdministrationService.EditKata(UserId, kata, cancellationToken);
    }

    [HttpDelete]
    public async Task DeleteKata([FromQuery] long kataId, CancellationToken cancellationToken)
    {
        await _kataAdministrationService.DeleteKata(UserId, kataId, cancellationToken);
    }
}