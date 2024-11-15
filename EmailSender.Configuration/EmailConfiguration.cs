namespace TourOperator.Configuration
{
    public class EmailConfiguration
    {
        public string SmtpServer { get; set; }
        public string SmtpPort { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string SenderPassword { get; set; }
    }
}