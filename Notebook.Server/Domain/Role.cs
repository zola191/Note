using Notebook.Server.Enum;

namespace Notebook.Server.Domain
{
    public class Role
    {
        public RoleName RoleName { get; set; }
        public List<User> Users { get; set; }

        public Role()
        {
            Users = new List<User>();
        }
    }
}
