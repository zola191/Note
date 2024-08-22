namespace Notebook.Server.Dto
{
    public class UserModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public List<NoteModel> Notes { get; set; }
        public List<RoleModel> RoleModels { get; set; }
    }
}
