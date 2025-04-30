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
                        "http://guess-code.ru", 
                        "http://api.guess-code.ru",
                        "185.102.139.119",
                        "127.0.0.1",
                        "localhost",
                        "45.43.88.41"
                    )
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }
}