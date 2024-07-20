namespace Notebook.Server.Dto
{
    public class RestoreUserModel
    {
        public UserModel model { get; set; }
        public string Token {  get; set; }
        public DateTime Validity { get; set; }
    }
}
