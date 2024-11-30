using GuessCode.API.Models.V1.Settings;

namespace GuessCode.API.Configurations;

public static class SettingsConfiguration
{
    public static void AddSettingsConfiguration(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<CredentialSettings>(builder.Configuration.GetSection("CredentialSettings"));
    }
}