using GuessCode.DAL.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GuessCode.Scheduler.Configurations;

public static class DatabaseConfiguration
{
    public static void AddDatabaseConfiguration(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<GuessContext>(options =>
        {
            options.UseNpgsql(builder.Configuration["GuessDb"]);
        });
    }
}