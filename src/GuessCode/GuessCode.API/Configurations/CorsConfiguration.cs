namespace GuessCode.API.Configurations;

public static class CorsConfiguration
{
    public static void AddCorsConfiguration(this IHostApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("GuessCodeCorsPolicy", corsBuilder =>
            {
                corsBuilder.WithOrigins("http://guess-code.site") 
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }
}