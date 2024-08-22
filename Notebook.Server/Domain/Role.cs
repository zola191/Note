using Notebook.Server.Enum;

namespace Notebook.Server.Domain
{
    public class Role
    {
        public RoleName RoleName { get; set; }
        public List<User> User { get; set; }

        public Role()
        {
            User = new List<User>();
        }
    }
}
