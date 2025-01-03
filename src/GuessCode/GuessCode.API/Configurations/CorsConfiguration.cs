namespace GuessCode.API.Configurations;

public static class CorsConfiguration
{
    public static void AddCorsConfiguration(this IHostApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", corsBuilder =>
            {
                corsBuilder.WithOrigins("http://localhost:3000", "http://192.168.0.35:3000", "http://guess-code-ui:3000") 
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }
}