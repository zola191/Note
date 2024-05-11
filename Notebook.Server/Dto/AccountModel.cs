using System.Text.Json.Serialization;

namespace Notebook.Server.Dto
{
    public class AccountModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        [JsonIgnore]
        public UserModel User { get; set; }

    }
}
