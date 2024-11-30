using StackExchange.Redis;

namespace GuessCode.Scheduler.Configurations;

public static class RedisConfiguration
{
    public static void AddRedisConfiguration(this IHostApplicationBuilder builder)
    {
        var connectionHost = builder.Configuration["Redis:Host"]!;
        builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var connection = ConnectionMultiplexer.Connect(connectionHost);
            return connection;
        });
    }
}