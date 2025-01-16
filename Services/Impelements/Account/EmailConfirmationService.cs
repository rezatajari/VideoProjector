using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace VideoProjector.Services.Impelements.Account
{
    public class EmailConfirmationService(IConfiguration configuration, ILogger<EmailConfirmationService> logger)
    {
        public async Task SendConfirmationEmailAsync(string toEmail, string subject, string message)
        {
            var smtpSettings = configuration.GetSection("SmtpSettings");

            var smtpClient = new SmtpClient(smtpSettings["Server"])
            {
                Port = int.Parse(smtpSettings["Port"]),
                Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpSettings["SenderEmail"], smtpSettings["SenderName"]),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                logger.LogInformation("Confirmation email sent to {Email}", toEmail);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sending confirmation email to {Email}", toEmail);
            }
        }
    }
}