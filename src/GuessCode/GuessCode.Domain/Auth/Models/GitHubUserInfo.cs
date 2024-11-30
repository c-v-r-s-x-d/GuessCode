using Newtonsoft.Json;

namespace GuessCode.Domain.Auth.Models;

public class GitHubUserInfo
{
    public string Id { get; set; }
    public string Login { get; set; }
    public string Email { get; set; }
    
    [JsonProperty("avatar_url")]
    public string AvatarUrl { get; set; }
}