using Microsoft.AspNetCore.Identity.Data;
using Notebook.Server.Domain;
using Notebook.Server.Dto;

namespace Notebook.Server.Services
{
    public interface IAccountService
    {
        Task<AccountModel> CreateAsync(AccountRequest request);
        Task<AccountModel> FindByEmail(string email);
        string GetUserEmail(HttpRequest request);
        Task<RestoreAccountModel> RestorePassword(AccountRestoreRequest request);
        Task<AccountModel> CheckLogin(AccountRequest request);
        bool IsExpired(string token);
        Task ChangePasswordAsync(ChangePasswordModel model);
        Task<Account> FindByToken(string token);
    }
}
