using Notebook.Server.Dto;

namespace Notebook.Server.Services
{
    public interface IAdminService
    {
        public Task<List<UserModel>> GetAllUsersAsync();
        public Task<UserModel> GetCurrentUserAsync(string email);
        public Task<UserModel> UpdateUserAsync(UserModelRequest user);
        public Task DeleteUserAsync(string email);
        public Task<List<RoleModel>> GetRolesAsync();
        public Task<bool> UpdateNoteAsync(int id, NoteRequest request);
        public Task<NoteModel> GetCurrenNoteAsync(int id);
        public Task DeleteNoteAsync(int id);

    }
}
