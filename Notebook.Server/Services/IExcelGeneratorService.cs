using Notebook.Server.Domain;

namespace Notebook.Server.Services
{
    public interface IExcelGeneratorService
    {
        public byte[] GenerateExcel(List<NoteChangeLog> noteChangeLogs, string email);
    }
}
