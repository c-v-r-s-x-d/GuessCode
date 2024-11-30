using System.ComponentModel.DataAnnotations;
using System.Web;
using GuessCode.API.Models.V1.Settings;
using GuessCode.DAL.Contexts;
using GuessCode.DAL.External.Contracts;
using GuessCode.DAL.Models.UserAggregate;
using GuessCode.Domain.Auth.Contracts;
using GuessCode.Domain.Auth.Models;
using GuessCode.Domain.Contracts;
using GuessCode.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GuessCode.Domain.Auth.Services;

public class GitHubLoginService : IGitHubLoginService
{
    private readonly IHttpService _httpService;
    private readonly GuessContext _guessContext;
    private readonly IUserRegisterService _registerService;
    private readonly IUserLoginService _loginService;
    private readonly GitHubSettings _gitHubSettings;

    public GitHubLoginService(IOptions<GitHubSettings> gitHubSettings, IHttpService httpService,
        IUserService userService, GuessContext guessContext, IUserRegisterService registerService,
        IUserLoginService loginService)
    {
        _httpService = httpService;
        _guessContext = guessContext;
        _registerService = registerService;
        _loginService = loginService;
        _gitHubSettings = gitHubSettings.Value;
    }

    public async Task<Token> Login(string code, CancellationToken cancellationToken)
    {
        var gitHubToken = await GetAccessToken(code, cancellationToken);

        if (gitHubToken is null)
        {
            throw new ValidationException("Error while github initializing");
        }

        var gitHubUserInfo = await GetGitHubUserInfo(gitHubToken, cancellationToken);

        var currentUser = await GetUserByGitHubLogin(gitHubUserInfo.Login, cancellationToken);

        if (currentUser is null)
        {
            var password = PasswordUtils.GeneratePassword(8);

            await _registerService.Register(gitHubUserInfo.Email, $"{gitHubUserInfo.Login}_{Guid.NewGuid()}",
                password, cancellationToken);

            currentUser = await _guessContext
                .Set<User>()
                .AsNoTracking()
                .Include(x => x.Role)
                .SingleAsync(x => x.Email == gitHubUserInfo.Email, cancellationToken);

            var gitHubProfile = new GitHubProfile
            {
                Login = gitHubUserInfo.Login,
                UserId = currentUser.Id
            };
            await _guessContext.AddAsync(gitHubProfile, cancellationToken);
        }
        
        //var accessToken =  await _loginService.GetAccessToken(currentUser.Username, password, )
        
        return new Token
        {
            AccessToken = null,
            UserId = null
        };
    }

    private async Task<string?> GetAccessToken(string code, CancellationToken cancellationToken)
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "client_id", _gitHubSettings.ClientId },
            { "client_secret", _gitHubSettings.ClientSecret },
            { "code", code },
            { "redirect_uri", _gitHubSettings.RedirectUri }
        });

        var response = await _httpService.PostAsync<string>(_gitHubSettings.AccessTokenUri, content, cancellationToken);
        var token = HttpUtility.ParseQueryString(response)["access_token"];

        return token;
    }

    private async Task<GitHubUserInfo> GetGitHubUserInfo(string accessToken, CancellationToken cancellationToken)
    {
        _httpService.AddDefaultRequestHeader("Authorization", $"Bearer: {accessToken}");
        _httpService.AddDefaultRequestHeader("User-Agent", "GuessCode");

        return await _httpService.GetAsync<GitHubUserInfo>(_gitHubSettings.GetUserInfoUri, cancellationToken);
    }

    private async Task<User?> GetUserByGitHubLogin(string gitHubLogin, CancellationToken cancellationToken)
    {
        return await _guessContext
            .Set<GitHubProfile>()
            .Where(x => x.Login == gitHubLogin)
            .Select(x => x.User)
            .SingleOrDefaultAsync(cancellationToken);
    }
}