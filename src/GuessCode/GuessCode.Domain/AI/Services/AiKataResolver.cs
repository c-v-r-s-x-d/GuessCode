using GroqSharp.Models;
using GuessCode.API.Models.V1.Settings;
using GuessCode.Domain.AI.Contracts;
using Microsoft.Extensions.Options;

namespace GuessCode.Domain.AI.Services;

public class AiKataResolver : IAiKataResolver
{
    private readonly IOptions<AISettings> _aiSettings;
    private const string DefaultPromptPrefix = "Напиши только код, решающий эту задачу, без каких либо объяснений: ";
    
    public AiKataResolver(IOptions<AISettings> aiSettings)
    {
        _aiSettings = aiSettings;
    }

    public async Task<string> ResolveKata(string kataDescription, CancellationToken cancellationToken)
    {
        var aiClient = new GroqClient(_aiSettings.Value.ApiKey, _aiSettings.Value.Model)
            .SetTemperature(0.5)
            .SetMaxTokens(256)
            .SetTopP(1)
            .SetStop("NONE");

        var result = await aiClient.CreateChatCompletionAsync(new Message
        {
            Role = MessageRoleType.User,
            Content = DefaultPromptPrefix + kataDescription
        });
        
        return result[(result.IndexOf('\n') + 1)..];
    }
}