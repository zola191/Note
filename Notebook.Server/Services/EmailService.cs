using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using MimeKit.Text;
using Notebook.Server.Dto;

namespace Notebook.Server.Services
{
    public class EmailService : IEmailService
    {
        public void SendEmail(EmailModel request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("kameron.funk35@ethereal.email"));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body};

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("kameron.funk35@ethereal.email", "xwvfTrdeXAe1NZbkM5");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
