namespace Notebook.Server.Services
{
    public interface IPasswordHasher
    {
        public string CreateSalt();
        string HashPassword(string password, string salt);
    }
}
