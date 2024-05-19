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
            email.From.Add(MailboxAddress.Parse("salma.kiehn5@ethereal.email"));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body};

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("salma.kiehn5@ethereal.email", "traFqUK3sXfVc6cqkz");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
