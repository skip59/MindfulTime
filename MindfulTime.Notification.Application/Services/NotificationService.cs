using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace MindfulTime.Notification.Services;

public class NotificationService : ISendMessage
{
    private readonly IConfiguration _configuration;
    internal static ITelegramBotClient client;
    internal static List<Update> upd = [];

    public NotificationService(IConfiguration configuration)
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

    public async Task<bool> SendMessageTg(SendModel item)
    {
        try
        {
            if (upd.Count > 0)
            {
                string message = string.Empty;
                foreach (Update update in upd)
                {
                    if (update.Message.Chat.Id == item.TelegramId)
                    {
                        message = $"{update.Message.From.FirstName}. {item.Message}";
                        await PRTelegramBot.Helpers.Message.Send(client, update, message);
                    }
                }
                return true;
            }
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
