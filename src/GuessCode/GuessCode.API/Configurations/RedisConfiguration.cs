using StackExchange.Redis;

namespace GuessCode.API.Configurations;

public static class RedisConfiguration
{
    public static void AddRedisConfiguration(this IHostApplicationBuilder builder)
    {
        var redisConnectionString = builder.Configuration["Redis:Host"]!;
        Console.WriteLine($"Redis connection string: {redisConnectionString}");

        builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));
        Console.WriteLine("RedisConnected");
    }
}