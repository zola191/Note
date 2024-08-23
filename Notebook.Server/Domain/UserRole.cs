using Notebook.Server.Enum;

namespace Notebook.Server.Domain
{
    public class UserRole
    {
        public RoleName RolesId { get; set; }
        public Role Role { get; set; }
        public string UsersId { get; set; }
        public User User { get; set; }
    }
}
