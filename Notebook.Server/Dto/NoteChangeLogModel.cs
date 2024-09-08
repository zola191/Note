namespace Notebook.Server.Dto
{
    public class NoteChangeLogModel
    {
        public long Id { get; set; }
        public DateTime ChangedAt { get; set; }
        public string Log { get; set; }
        public string Email { get; set; }
    }
}
