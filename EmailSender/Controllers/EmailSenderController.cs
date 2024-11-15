using Microsoft.AspNetCore.Mvc;
using TourOperator.Services;

namespace TourOperator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly EmailService _emailService;

        public EmailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send-email")]
        public IActionResult SendEmail([FromForm] string recipientEmail, [FromForm] string subject, [FromForm] string body)
        {
            try
            {
                _emailService.SendEmail(recipientEmail, subject, body);
                return Ok("Email sent successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error sending email: {ex.Message}");
            }
        }
    }
}