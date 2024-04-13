using Notebook.Server.Dto;

namespace Notebook.Server.Services
{
    public interface INotebookService
    {
        Task<NoteModel> CreateAsync(NoteRequest request);
        Task<IEnumerable<NoteModel>> GetAllAsync();
        Task<NoteModel> GetById(int id);
        Task<NoteModel> UpdateAsync(int id, NoteRequest request);
        Task<NoteModel> DeleteAsync(int id);
    }
}
