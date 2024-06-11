namespace Notebook.Server.Services
{
    public interface IPasswordHasher
    {
        public string CreateSalt();
        public string HashPassword(string password);
    }
}
