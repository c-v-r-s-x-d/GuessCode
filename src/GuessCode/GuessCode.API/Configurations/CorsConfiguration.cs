namespace GuessCode.API.Configurations;

public static class CorsConfiguration
{
    public static void AddCorsConfiguration(this IHostApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", corsBuilder =>
            {
                corsBuilder.WithOrigins("http://localhost:3001", "192.168.0.35", "localhost", "127.0.0.1", "0.0.0.0") 
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }
}