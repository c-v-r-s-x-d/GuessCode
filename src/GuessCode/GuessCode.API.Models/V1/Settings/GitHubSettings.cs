namespace GuessCode.API.Models.V1.Settings;

public class GitHubSettings
{
    public string ClientId { get; set; } = null!;

    public string ClientSecret { get; set; } = null!;

    public string RedirectUri { get; set; } = null!;

    public string AccessTokenUri { get; set; } = null!;

    public string GetUserInfoUri { get; set; } = null!;
}