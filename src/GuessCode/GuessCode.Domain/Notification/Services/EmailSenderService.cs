using System.Net;
using System.Net.Mail;
using GuessCode.Domain.Notification.Contracts;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace GuessCode.Domain.Notification.Services;

public class EmailSenderService : IEmailSenderService
{
    private readonly IConfiguration _configuration;
    private readonly IConnectionMultiplexer _redis;

    private readonly string _emailFrom;

    public EmailSenderService(IConfiguration configuration, IConnectionMultiplexer redis)
    {
        _configuration = configuration;
        _redis = redis;
        _emailFrom = _configuration["EmailCredentials:FromMail"]!;
    }
    
    public Task SendEmailNotifications(string notificationType, IReadOnlyDictionary<string, object[]> receiverData)
    {
        var notificationSubject = _configuration[$"Emails:{notificationType}:Subject"]!;
        var notificationContent = _configuration[$"Emails:{notificationType}:Content"]!;
        Console.WriteLine($"Notification Type - {notificationType}");
        Console.WriteLine($"Notification Content - {notificationContent}");
        
        var smtpClient = CreateSmtpClient();

        Parallel.ForEach(receiverData.Keys,
            x => SendEmail(smtpClient, notificationSubject, FormatNotification(notificationContent, receiverData[x]),
                x, notificationType));
        return Task.CompletedTask;
    }

    private void SendEmail(SmtpClient client, string subject, string content, string receiverEmail, string notificationType)
    {
        var mailMessage = new MailMessage(
            from: _emailFrom,
            to: receiverEmail,
            subject: subject,
            body: content);
        mailMessage.IsBodyHtml = true;

        try
        {
            Console.WriteLine($"Начинается отправка письма для {mailMessage.To}...");

            client.Send(mailMessage);
            Console.WriteLine($"Письмо с темой: \"{mailMessage.Subject}\" отправлено");

            var database = _redis.GetDatabase();
            database.KeyDelete(notificationType);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при отправке письма: " + ex.Message);
        }
    }
    
    private SmtpClient CreateSmtpClient()
    {
        var smtpClient = _configuration["EmailCredentials:SmtpHost"];
        var port = Convert.ToInt32(_configuration["EmailCredentials:SmtpPort"]);
        var useDefaultCredentials = Convert.ToBoolean(_configuration["EmailCredentials:UseDefaultCredentials"]);
        var enableSsl = Convert.ToBoolean(_configuration["EmailCredentials:EnableSsl"]);
        var withPassword = _configuration["EmailCredentials:WithPassword"];
        
        var client = new SmtpClient(smtpClient);
        client.Port = port;
        client.UseDefaultCredentials = useDefaultCredentials;
        client.Credentials = new NetworkCredential(
            _emailFrom,
            withPassword);
        client.EnableSsl = enableSsl;
        return client;
    }

    // 0 - Username
    private static string FormatNotification(string template, object[] customData)
    {
        return string.Format(template, customData);
    }
}