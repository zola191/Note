using Notebook.Server.Dto;

namespace Notebook.Server.Services
{
    public interface IEmailService
    {
        void SendEmail(EmailModel request);
    }
}
