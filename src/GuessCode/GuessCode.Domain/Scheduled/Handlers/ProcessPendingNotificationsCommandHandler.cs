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

        var welcomeRequest = await database.StringGetAsync(nameof(SendWelcomeEmailCommand));
        var rejectedRequest = await database.StringGetAsync(nameof(SendRejectedMentorshipEmailCommand));
        var approvedRequest = await database.StringGetAsync(nameof(SendApprovedMentorshipEmailCommand));

        await SendEmailWithUsername<SendWelcomeEmailCommand>(welcomeRequest);
        await SendEmailWithUsername<SendRejectedMentorshipEmailCommand>(rejectedRequest);
        await SendEmailWithUsername<SendApprovedMentorshipEmailCommand>(approvedRequest);
    }

    private async ValueTask SendEmailWithUsername<T>(RedisValue value) where T : BaseEmail, IRequest
    {
        if (value.IsNullOrEmpty)
        {
            return;
        }

        var requests = JsonConvert.DeserializeObject<T[]>(value!);
        await _emailSenderService.SendEmailNotifications(nameof(T),
            requests!.ToDictionary(x => x.ReceiverEmail, x => new object[] { x.Username }));
    }
}