using Notebook.Server.Domain;
using Notebook.Server.Dto;

namespace Notebook.Server.Authentication
{
    public interface IJwtProvider
    {
        string Generate(UserModel account);
        string GenerateRestore(User user);
    }
}
