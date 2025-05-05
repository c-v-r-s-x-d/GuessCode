using GuessCode.DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace GuessCode.API.Configurations;

public static class DbConfiguration
{
    public static void AddDbConfiguration(this IHostApplicationBuilder builder)
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

    public static void ApplyDatabaseMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<GuessContext>();
        context.Database.Migrate();
    }
}