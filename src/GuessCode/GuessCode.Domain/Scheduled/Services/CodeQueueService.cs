using System.Text.Json;
using GuessCode.Domain.Scheduled.Models;
using StackExchange.Redis;

namespace GuessCode.Domain.Scheduled.Services;

public class CodeQueueService
{
    private readonly IConnectionMultiplexer _redis;

    public CodeQueueService(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task EnqueueAsync(CodeExecutionTask task)
    {
        var db = _redis.GetDatabase();
        var json = JsonSerializer.Serialize(task);
        await db.ListRightPushAsync("code:queue", json);
    }

    public async Task<CodeExecutionTask?> DequeueAsync()
    {
        var db = _redis.GetDatabase();
        var result = await db.ListLeftPopAsync("code:queue");

        return result.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CodeExecutionTask>(result!);
    }
}