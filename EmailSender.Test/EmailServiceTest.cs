using EmailSender.Service.Interface;
using Microsoft.Extensions.Options;
using Moq;
using System.Net.Mail;
using TourOperator.Configuration;
using TourOperator.Services;
using Xunit;

namespace TourOperator.Tests.ServiceTests
{
    public class EmailServiceTest
    {
        private readonly Mock<IEmailClient> _mockEmailClient;
        private readonly EmailService _emailService;
        private readonly EmailConfiguration _emailConfig;

        public EmailServiceTest()
        {
            _mockEmailClient = new Mock<IEmailClient>();
            _emailConfig = new EmailConfiguration
            {
                SmtpServer = "smtp.test.com",
                SmtpPort = "587",
                SenderEmail = "test@example.com",
                SenderPassword = "password",
                SenderName = "Test Sender"
            };

            _emailService = new EmailService(_mockEmailClient.Object, Options.Create(_emailConfig));
        }

        [Fact]
        public void SendEmail_ShouldCallEmailClientSend()
        {
            var recipientEmail = "recipient@example.com";
            var subject = "Test Subject";
            var body = "Test Body";

            // Act
            _emailService.SendEmail(recipientEmail, subject, body);

            // Assert
            _mockEmailClient.Verify(client => client.Send(It.IsAny<MailMessage>()), Times.Once);
        }

        [Fact]
        public void SendEmail_InvalidSmtpPort_ShouldThrow()
        {
            var mockInvalidConfig = new Mock<IOptions<EmailConfiguration>>();
            mockInvalidConfig.Setup(config => config.Value).Returns(new EmailConfiguration
            {
                SmtpServer = "smtp.test.com",
                SmtpPort = "invalid",
                SenderEmail = "test@example.com",
                SenderPassword = "password",
                SenderName = "Test Sender"
            });

            var invalidEmailService = new EmailService(_mockEmailClient.Object, mockInvalidConfig.Object);

            Assert.Throws<InvalidOperationException>(() =>
                invalidEmailService.SendEmail("recipient@example.com", "Subject", "Body"));
        }

        [Fact]
        public void SendEmail_ShouldThrow_WhenRecipientIsEmpty()
        {
            var recipientEmail = "";
            var subject = "Test Subject";
            var body = "Test Body";

            Assert.Throws<ArgumentException>(() => _emailService.SendEmail(recipientEmail, subject, body));
        }
    }
}