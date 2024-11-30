using AutoMapper;
using GuessCode.API.Models.V1.Auth;
using GuessCode.API.Models.V1.Settings;
using GuessCode.Domain.Auth.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GuessCode.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : BaseGuessController
{
    private readonly IMapper _mapper;
    private readonly IUserLoginService _loginService;
    private readonly IUserRegisterService _registerService;
    private readonly IGitHubLoginService _gitHubLoginService;
    private readonly CredentialSettings _credentialSettings;

    public AuthController(IMapper mapper, IUserLoginService loginService, IUserRegisterService registerService,
        IOptions<CredentialSettings> credentialSettings, IGitHubLoginService gitHubLoginService)
    {
        _loginService = loginService;
        _mapper = mapper;
        _registerService = registerService;
        _gitHubLoginService = gitHubLoginService;
        _credentialSettings = credentialSettings.Value;
    }

    [HttpPost("login")]
    public async Task<TokenDto> Login([FromBody] LoginDto loginDto, CancellationToken cancellationToken)
    {
        var token = await _loginService.GetAccessToken(loginDto.Username, loginDto.Password,
            _credentialSettings.SecretKey, _credentialSettings.Issuer, _credentialSettings.Audience, cancellationToken);
        return _mapper.Map<TokenDto>(token);
    }

    [HttpPost("login/github")]
    public async Task<TokenDto> LoginGitHub([FromBody] string code, CancellationToken cancellationToken)
    {
        var token = await _gitHubLoginService.Login(code, cancellationToken);
        return _mapper.Map<TokenDto>(token);
    }
    
    [HttpPost("register")]
    public async Task Register([FromBody] RegisterDto registerDto, CancellationToken cancellationToken)
    {
        await _registerService.Register(registerDto.Email, registerDto.Username, registerDto.Password, cancellationToken);
    }
}