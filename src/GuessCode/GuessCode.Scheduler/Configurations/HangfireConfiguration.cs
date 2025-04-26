using GuessCode.Domain.Contracts;
using GuessCode.Domain.Notification.Contracts;
using GuessCode.Domain.Notification.Services;
using GuessCode.Domain.Services;
using GuessCode.Scheduler.Contracts;
using GuessCode.Scheduler.Models;
using GuessCode.Scheduler.Services;
using GuessCode.Scheduler.Temp;
using Hangfire;
using Hangfire.Redis.StackExchange;

namespace GuessCode.Scheduler.Configurations;

public static class HangfireConfiguration
{
    private const string ScheduledSection = "ScheduledCommands";
    
    public static void AddHangfireConfiguration(this IHostApplicationBuilder builder)
    {
        var connectionHost = builder.Configuration["Redis:Host"]!;
        
        builder.Services.AddHangfire(config => config.UseRedisStorage(connectionHost));

        builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
        builder.Services.AddScoped<IUserStatusUpdateService, UserStatusUpdateService>();
        builder.Services.AddSingleton<IJobService, JobService>();
        builder.Services.AddHangfireServer();
    }
    
    public static void ConfigureHangfire(this WebApplication app)
    {
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = [new AuthWorkaroundFilterAttribute()]
        });
#pragma warning disable CS0618
        app.UseHangfireServer(new BackgroundJobServerOptions
        {
            WorkerCount = 1
        });
#pragma warning restore CS0618

        app.MapFallback(context =>
        {
            context.Response.Redirect("/hangfire", permanent: true);
            return Task.CompletedTask;
        });
        
        var scheduledJobs = app.Configuration
            .GetSection(ScheduledSection)
            .Get<List<ScheduledCommand>>()!;

        foreach (var commandData in scheduledJobs)
        {
            var commandType = Type.GetType(commandData.Name);
            
            if (commandType == null)
            {
                throw new InvalidOperationException($"Command type '{commandData.Name}' not found.");
            }

            RecurringJob.AddOrUpdate(
                commandData.Name,
                () => app.Services.GetRequiredService<IJobService>().ExecuteJob(commandType),
                commandData.Schedule
            );
        }
    }

}