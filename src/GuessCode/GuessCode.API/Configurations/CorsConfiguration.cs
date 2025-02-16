namespace GuessCode.API.Configurations;

public static class CorsConfiguration
{
    public static void AddCorsConfiguration(this IHostApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("GuessCodeCorsPolicy", corsBuilder =>
            {
                corsBuilder.WithOrigins(
                        "http://guess-code.site", 
                        "http://api.guess-code.site",
                        "10.43.97.6"
                    )
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }
}