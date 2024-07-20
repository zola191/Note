namespace Notebook.Server.Dto
{
    public class ChangeUserPasswordModel
    {
        public string Token { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
