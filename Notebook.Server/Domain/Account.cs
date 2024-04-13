using System.ComponentModel.DataAnnotations;

namespace Notebook.Server.Domain
{
    public class Account
    {
        [Key]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    }
}
