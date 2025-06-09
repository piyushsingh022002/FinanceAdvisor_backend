using System.Net.Mail;
using System.Threading.Tasks;
using FinanceAdvisorApi.Models;
using Microsoft.Extensions.Configuration;

namespace FinanceAdvisorApi.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendAsync(EmailMessage message)
        {
            using var client = new SmtpClient(
                _configuration["EmailSettings:SmtpServer"],
                int.Parse(_configuration["EmailSettings:SmtpPort"]))
            {
                Credentials = new System.Net.NetworkCredential(
                    _configuration["EmailSettings:SenderEmail"],
                    _configuration["EmailSettings:SenderPassword"]),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(
                    _configuration["EmailSettings:SenderEmail"],
                    _configuration["EmailSettings:SenderName"]),
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(message.To);

            await client.SendMailAsync(mailMessage);
        }
    }
}