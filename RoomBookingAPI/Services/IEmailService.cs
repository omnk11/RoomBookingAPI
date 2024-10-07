using MimeKit;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace RoomBookingAPI.Services
{
    // RoomBookingAPI/Services/IEmailService.cs
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }

    // RoomBookingAPI/Services/EmailService.cs


    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(emailSettings.GetValue<string>("FromEmail")));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(emailSettings.GetValue<string>("SmtpServer"), emailSettings.GetValue<int>("SmtpPort"), MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(emailSettings.GetValue<string>("SmtpUser"), emailSettings.GetValue<string>("SmtpPass"));
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }

}
