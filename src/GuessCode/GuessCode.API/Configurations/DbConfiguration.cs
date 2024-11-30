using GuessCode.DAL.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GuessCode.API.Configurations;

public static class DbConfiguration
{
    public static void AddDbConfiguration(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<GuessContext>(options =>
        {
            options.UseNpgsql(builder.Configuration["GuessDb"]);
        });
    }

    public static void ApplyDatabaseMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<GuessContext>();
        context.Database.Migrate();
    }
}