namespace Notebook.Server.Domain
{
    public class RestoreUser
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Token {  get; set; }
        public DateTime Validity { get; set; }
    }
}
