namespace Notebook.Server.Domain
{
    public class Account
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public User User { get; set; }
    }
}
