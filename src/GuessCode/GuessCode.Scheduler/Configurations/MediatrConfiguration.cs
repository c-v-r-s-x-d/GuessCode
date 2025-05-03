using System.Reflection;
using GuessCode.Domain.Scheduled.Handlers;
using GuessCode.Domain.Scheduled.Requests;
using MediatR;
using MediatR.Registration;

namespace GuessCode.Scheduler.Configurations;

public static class MediatrConfiguration
{
    public static void AddMediatrConfiguration(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        ServiceRegistrar.AddRequiredServices(builder.Services, new MediatRServiceConfiguration());
        builder.Services
            .AddScoped<IRequestHandler<ProcessPendingNotificationsCommand>,
                ProcessPendingNotificationsCommandHandler>();
        builder.Services
            .AddScoped<IRequestHandler<UpdateUserActivityStatusesCommand>,
                UpdateUserActivityStatusesCommandHandler>();
        builder.Services
            .AddScoped<IRequestHandler<ProcessNextCodeExecutionRequestCommand>,
                ProcessNextCodeExecutionRequestCommandHandler>();
    }
}