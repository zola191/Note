
namespace Notebook.Server.Dto
{
    public class NoteRequest
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? Organization { get; set; }
        public string? Position { get; set; }
        public string? Other { get; set; }

    }
}
