﻿
namespace Notebook.Server.Dto
{
    public class AccountModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public UserModel User { get; set; }
        public IEnumerable<NoteModel> Notes { get; set; }
    }
}
