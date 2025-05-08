using GuessCode.DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace GuessCode.Scheduler.Configurations;

public static class DatabaseConfiguration
{
    public static void AddDatabaseConfiguration(this IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration["GuessDb"] 
                               ?? throw new InvalidOperationException("Connection string 'GuessDb' not found.");
        
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();

        // регистрируем DbContext с кастомным data source
        builder.Services.AddDbContext<GuessContext>(options =>
        {
            options.UseNpgsql(dataSource);
        });
    }
}