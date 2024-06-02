namespace Notebook.Server.Dto
{
    public class ChangePasswordModel
    {
        public string Token { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
