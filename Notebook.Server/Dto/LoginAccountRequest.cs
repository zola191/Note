using Microsoft.AspNetCore.Authentication;

namespace Notebook.Server.Dto
{
    public class LoginAccountRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
