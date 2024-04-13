using Notebook.Server.Dto;

namespace Notebook.Server.Services
{
    public interface IAccountService
    {
        Task<AccountModel> CreateAsync(AccountRequest request);
        Task<AccountModel> GetByEmail(string email);
    }
}
