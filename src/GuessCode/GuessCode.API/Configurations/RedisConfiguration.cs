using System.ComponentModel.DataAnnotations;
using StackExchange.Redis;

namespace GuessCode.API.Configurations;

public static class RedisConfiguration
{
    public static void AddRedisConfiguration(this IHostApplicationBuilder builder)
    {
        var redisHost = builder.Configuration["Redis:Host"]!;
        var redisPassword = builder.Configuration["Redis:Password"]!;

        /*if (string.IsNullOrEmpty(redisHost) || string.IsNullOrEmpty(redisPassword))
        {
            throw new ValidationException("Nullable redis credentials is prohibited");
        }*/
        
        //Environment.SetEnvironmentVariable("REDIS_PASSWORD", redisPassword);

        var redisConfiguration = ConfigurationOptions.Parse(redisHost);
        redisConfiguration.Password = redisPassword;
        redisConfiguration.AbortOnConnectFail = false;
        redisConfiguration.ConnectRetry = 5;

        builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfiguration));
    }
}