namespace Notebook.Server.Dto
{
    public class UserModelRequest
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<RoleModel> RoleModels { get; set; }
    }
}
