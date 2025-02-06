using System.Net.Mail;
using System.Net;

namespace carGooBackend.Services
{
    // Services/EmailService.cs
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            var mailMessage = new MailMessage
            {
                From = new MailAddress(emailSettings["SenderEmail"], emailSettings["SenderName"]),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            using (var client = new SmtpClient(emailSettings["SmtpServer"], int.Parse(emailSettings["SmtpPort"])))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(emailSettings["SmtpUsername"], emailSettings["SmtpPassword"]);
                await client.SendMailAsync(mailMessage);
            }
        }
    }

}
