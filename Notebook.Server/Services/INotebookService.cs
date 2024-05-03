using Notebook.Server.Dto;

namespace Notebook.Server.Services
{
    public interface INotebookService
    {
        Task<NoteModel> CreateAsync(NoteRequest request, string email);
        Task<IEnumerable<NoteModel>> GetAllAsync(string email);
        Task<NoteModel> GetById(int id,string email);
        Task<NoteModel> UpdateAsync(int id, NoteRequest request, string email);
        Task<NoteModel> DeleteAsync(int id);

    }
}
