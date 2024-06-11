using System.Security.Cryptography;
using System.Text;

namespace Notebook.Server.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        const int keySize = 64;
        const int interations = 400000;
        HashAlgorithmName hashAlgoritm = HashAlgorithmName.SHA256;

        public string CreateSalt()
        {
            var salt = RandomNumberGenerator.GetBytes(keySize);
            return Encoding.Default.GetString(salt);
        }

        public string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(keySize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password),
                                                salt, interations, hashAlgoritm, keySize);

            return Convert.ToHexString(hash);
        }
    }
}
