using GuessCode.Scheduler.Contracts;
using Hangfire;
using MediatR;

namespace GuessCode.Scheduler.Services;

public class JobService : IJobService
{
    private readonly IMediator _mediator;

    public JobService(IMediator mediator)
    {
        _mediator = mediator;
    }

    [JobDisplayName("{0}")]
    public async Task ExecuteJob(Type commandType)
    {
        var command = Activator.CreateInstance(commandType);

        if (command == null)
        {
            throw new InvalidOperationException($"Unable to create instance of command '{commandType.Name}'.");
        }
        
        await _mediator.Send(command);
    }
}