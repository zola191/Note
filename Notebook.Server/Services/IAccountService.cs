using Notebook.Server.Dto;

namespace Notebook.Server.Services
{
    public interface IAccountService
    {
        Task<AccountModel> CreateAsync(AccountRequest request);
        Task<AccountModel> FindByEmail(string email);
        string GetUserEmail(HttpRequest request);
    }
}
