using GuessCode.API.Models.V1.Settings;

namespace GuessCode.Scheduler.Configurations;

public static class SettingsConfiguration
{
    public static void AddSettingsConfiguration(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<CodeExecutionSettings>(builder.Configuration.GetSection("CodeExecutionSettings"));
    }
}