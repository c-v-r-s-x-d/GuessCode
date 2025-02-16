using StackExchange.Redis;

namespace GuessCode.Scheduler.Configurations;

public static class RedisConfiguration
{
    public static void AddRedisConfiguration(this IHostApplicationBuilder builder)
    {
        var redisConnectionString = builder.Configuration["Redis:Host"]!;
        builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));
    }
}