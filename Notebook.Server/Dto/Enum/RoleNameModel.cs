using System.ComponentModel.DataAnnotations;

namespace Notebook.Server.Dto.Enum
{
    public enum RoleNameModel
    {
        [Display(Name = "Admin")]
        Admin,
        [Display(Name = "User")]
        User
    }
}
