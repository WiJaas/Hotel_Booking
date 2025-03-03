using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

public class EmailSender : IEmailSender
{
    private readonly string _smtpServer = "smtp.example.com"; // Replace with your SMTP server
    private readonly int _smtpPort = 587; // Replace with your SMTP port
    private readonly string _smtpUsername = "your-email@example.com"; // Replace with your email
    private readonly string _smtpPassword = "your-password"; // Replace with your email password

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("YourApp", _smtpUsername));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = htmlMessage };
        message.Body = bodyBuilder.ToMessageBody();

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(_smtpServer, _smtpPort, false);
            await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
