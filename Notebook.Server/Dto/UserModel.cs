namespace Notebook.Server.Dto
{
    public class UserModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountId { get; set; }
        public AccountModel Account { get; set; }
        public List<NoteModel> Notes { get; set; }
    }
}
