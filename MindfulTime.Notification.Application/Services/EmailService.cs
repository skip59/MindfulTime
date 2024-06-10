using System.Net.Mail;
using System.Net;

namespace MindfulTime.Notification.Services;

public class EmailService : IMessageService<MailSendModel>
{
    public async Task<bool> SendMessage(MailSendModel user)
    {
        var smtpClient = new SmtpClient("smtp.mail.ru")
        {
            Port = 587,
            Credentials = new NetworkCredential("mindfultime@mail.ru", "sjSM0rBgNhpauJyk1y5Q"),
            EnableSsl = true,
        };
        var mailMessage = new MailMessage
        {
            From = new MailAddress("mindfultime@mail.ru"),
            Subject = "Test email",
            Body = "body",
            IsBodyHtml = true,
        };
        mailMessage.To.Add(user.Mail);

        smtpClient.Send(mailMessage);

        return true;
    }
}
