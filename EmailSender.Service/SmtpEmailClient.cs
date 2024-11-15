using EmailSender.Service.Interface;
using System.Net;
using System.Net.Mail;
using TourOperator.Configuration;
using TourOperator.Services;

public class SmtpEmailClient : IEmailClient
{
    private readonly EmailConfiguration _emailConfig;

    public SmtpEmailClient(EmailConfiguration emailConfig)
    {
        _emailConfig = emailConfig;
    }

    public void Send(MailMessage message)
    {
        if (!int.TryParse(_emailConfig.SmtpPort, out int smtpPort))
        {
            throw new InvalidOperationException("Invalid SMTP port configuration.");
        }

        using (var smtpClient = new SmtpClient(_emailConfig.SmtpServer)
        {
            Port = smtpPort,
            Credentials = new NetworkCredential(_emailConfig.SenderEmail, _emailConfig.SenderPassword),
            EnableSsl = true
        })
        {
            smtpClient.Send(message);
        }
    }
}
