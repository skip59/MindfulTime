using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace MindfulTime.Notification.Services;

public class EmailService : ISendMessageEmail
{
    private readonly IConfiguration _configuration;
    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> SendMessage(SendModel user)
    {
		try
		{
            var emailSettings = _configuration.GetSection(nameof(EmailSettings)).Get<EmailSettings>();

            MimeMessage message = new();
            message.From.Add(new MailboxAddress("MindfulTime notification service", emailSettings.Email));
            message.To.Add(new MailboxAddress(user.UserName, user.Email));
            message.Subject = user.UserName;
            message.Body = new BodyBuilder() { HtmlBody = $"<div style=\"color: green;\">{user.Message}</div>" }.ToMessageBody();

            using SmtpClient client = new();
            client.Connect("smtp.yandex.ru", 465, true);
            client.Authenticate(emailSettings.Email, emailSettings.Password);
            await client.SendAsync(message);

            client.Disconnect(true);
            return true;
        }
		catch (Exception ex)
		{
            return false;
		}

    }
}
