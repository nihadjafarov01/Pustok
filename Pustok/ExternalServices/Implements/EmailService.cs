using System.Net;
using System.Net.Mail;
using WebApplication1.ExternalServices.Interfaces;

namespace WebApplication1.ExternalServices.Implements
{
    public class EmailService : IEmailService
    {
        IConfiguration _configuration {  get; }

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Send(string toMail, string header, string body, bool isHtml=true)
        {
            SmtpClient smtpClient = new SmtpClient(_configuration["Email:Host"], Convert.ToInt32(_configuration["Email:Port"]));
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(_configuration["Email:Username"], _configuration["Email:Password"]);

            MailAddress from = new MailAddress(_configuration["Email:Username"], "Pustok");
            MailAddress to = new MailAddress(toMail);

            MailMessage message = new MailMessage(from, to);
            message.Body = body;
            message.Subject = header;
            message.IsBodyHtml = isHtml;

            smtpClient.Send(message);
        }
    }
}
