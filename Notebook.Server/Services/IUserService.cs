using Notebook.Server.Dto;

namespace Notebook.Server.Services
{
    public interface IUserService
    {
        Task<UserModel> FindByEmail(string email);
        Task<UserModel> CreateAsync(AccountModel accountModel);
    }
}
