using GuessCode.API.Middlewares;
using Microsoft.OpenApi.Models;

namespace GuessCode.API.Configurations;

public static class PrimaryConfiguration
{
    public static void AddPrimaryConfiguration(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHttpClient();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        });
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<ApiExceptionHandler>();
        builder.Services.AddAutoMapper(typeof(Program));
        builder.Services.AddSignalR();
    }
}