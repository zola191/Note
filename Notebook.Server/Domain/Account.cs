using System.ComponentModel.DataAnnotations;

namespace Notebook.Server.Domain
{
    public class Account
    {
        //[Key]
        public string Email { get; set; }
        public string Password { get; set; }
        public string? UserId { get; set; }
        public User User { get; set; }
    }
}
