namespace Notebook.Server.Dto
{
    public class GoogleUserModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public List<NoteModel> Notes { get; set; }
    }
}
