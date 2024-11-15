using EmailSender.Service.Interface;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using TourOperator.Configuration;

namespace TourOperator.Services
{
    public class EmailService
    {
        private readonly IEmailClient _emailClient;
        private readonly EmailConfiguration _emailConfig;

        public EmailService(IEmailClient emailClient, IOptions<EmailConfiguration> emailConfig)
        {
            _emailClient = emailClient;
            _emailConfig = emailConfig.Value;
        }
        private void ValidateSmtpPort(string smtpPort)
        {
            if (!int.TryParse(smtpPort, out int port) || port < 1 || port > 65535)
            {
                throw new InvalidOperationException("Invalid SMTP port.");
            }
        }

        public void SendEmail(string recipientEmail, string subject, string body)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailConfig.SenderEmail, _emailConfig.SenderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            ValidateSmtpPort(_emailConfig.SmtpPort);
            mailMessage.To.Add(recipientEmail);
            _emailClient.Send(mailMessage);
        }
    }
}