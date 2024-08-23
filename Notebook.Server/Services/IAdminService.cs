using Notebook.Server.Dto;

namespace Notebook.Server.Services
{
    public interface IAdminService
    {
        public Task<List<UserModel>> GetAllUsers();
        public Task<UserModel> GetCurrentUser(string email);
        public Task<UserModel> UpdateUser(UserModelRequest user);
        public Task DeleteUser(string email);
        public Task<List<RoleModel>> GetRoles();

    }
}
