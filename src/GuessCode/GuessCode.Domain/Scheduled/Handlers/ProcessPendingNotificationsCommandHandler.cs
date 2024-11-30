using GuessCode.DAL.Commands;
using GuessCode.Domain.Notification.Contracts;
using GuessCode.Domain.Scheduled.Requests;
using MediatR;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace GuessCode.Domain.Scheduled.Handlers;

public class ProcessPendingNotificationsCommandHandler : IRequestHandler<ProcessPendingNotificationsCommand>
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IEmailSenderService _emailSenderService;

    public ProcessPendingNotificationsCommandHandler(IConnectionMultiplexer redis, IEmailSenderService emailSenderService)
    {
        _redis = redis;
        _emailSenderService = emailSenderService;
    }

    public async Task Handle(ProcessPendingNotificationsCommand request, CancellationToken cancellationToken)
    {
        var database = _redis.GetDatabase();

        var sendRequest = await database.StringGetAsync(nameof(SendWelcomeEmailCommand));

        if (sendRequest.IsNullOrEmpty)
        {
            return;
        }
        
        var welcomeRequests = JsonConvert.DeserializeObject<SendWelcomeEmailCommand[]>(sendRequest!)!;

        await _emailSenderService.SendEmailNotifications(nameof(SendWelcomeEmailCommand),
            welcomeRequests.ToDictionary(x => x.ReceiverEmail, x => new object[] { x.Username }));
    }
}