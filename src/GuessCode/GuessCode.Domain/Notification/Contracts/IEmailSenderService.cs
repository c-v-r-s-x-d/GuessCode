namespace GuessCode.Domain.Notification.Contracts;

public interface IEmailSenderService
{
    Task SendEmailNotifications(string notificationType, IReadOnlyDictionary<string, object[]> receiverData);
}