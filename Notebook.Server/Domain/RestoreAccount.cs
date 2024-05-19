namespace Notebook.Server.Domain
{
    public class RestoreAccount
    {
        public int Id { get; set; }
        public Account Account { get; set; }
        public string Token {  get; set; }
        public DateTime Validity { get; set; }
    }
}
