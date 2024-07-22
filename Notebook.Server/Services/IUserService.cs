using Notebook.Server.Domain;
using Notebook.Server.Dto;

namespace Notebook.Server.Services
{
    public interface IUserService
    {
        public Task<UserModel> CreateAsync(CreateUserRequest request);
        public Task<UserModel> DeleteAsync(UserModel model);
        public Task<UserModel> CheckUser(LoginAccountRequest request);
        public Task<UserModel> FindByEmail(string email);
        public Task ChangePasswordAsync(ChangeUserPasswordModel model);
        public Task<User> FindByToken(string token);
        public Task<UserModel> RestorePassword(RestoreUserModel model);
        public string GetUserEmail(HttpRequest request);
        public Task<UserModel> CheckGoogleUser(LoginWithGoogleRequest request);
    }
}
