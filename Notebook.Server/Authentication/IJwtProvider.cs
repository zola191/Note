using Notebook.Server.Dto;

namespace Notebook.Server.Authentication
{
    public interface IJwtProvider
    {
        string Generate(AccountModel account);
    }
}
