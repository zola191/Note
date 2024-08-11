using Notebook.Server.Domain;

namespace Notebook.Server.Services
{
    public interface IFileService
    {
        List<Note> DeserializeFromFile(IFormFile file);
    }
}
