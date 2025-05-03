using GuessCode.Domain.Rating.Contracts;
using GuessCode.Domain.Rating.Services;
using GuessCode.Domain.Scheduled.Services;

namespace GuessCode.Scheduler.Configurations;

public static class BusinessLogicConfiguration
{
    public static void AddBusinessLogicConfiguration(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<CodeExecutor>();
        builder.Services.AddScoped<CodeQueueService>();
        builder.Services.AddScoped<IRatingService, RatingService>();
    }
}