namespace Notebook.Server.Domain
{
    public class ExternalGoogleUser
    {
        public string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public ICollection<Note> Notes { get; set; }
        public ExternalGoogleUser()
        {
            Notes = new List<Note>();
        }
    }

}
