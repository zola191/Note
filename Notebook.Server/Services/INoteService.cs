using Notebook.Server.Dto;
using Notebook.Server.Domain;

namespace Notebook.Server.Services
{
    public interface INoteService
    {
        Task<NoteModel> CreateAsync(NoteRequest request, string email);
        Task<IEnumerable<NoteModel>> GetAllAsync(string email);
        Task<NoteModel> GetById(int id,string email);
        Task<NoteModel> UpdateAsync(int id, NoteRequest request, string email);
        Task<NoteModel> DeleteAsync(int id);
        Task<List<NoteModel>> CreateNotesAsync(List<Note> notes, string email);
    }
}
