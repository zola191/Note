using System.Security.Cryptography;
using System.Text;

namespace Notebook.Server.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;

        public string CreateSalt()
        {
            var salt = RandomNumberGenerator.GetBytes(keySize);

            return Convert.ToHexString(salt);
        }

        public string HashPassword(string password, string salt)
        {
            var saltBytes = Enumerable.Range(0, salt.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(salt.Substring(x, 2), 16))
                     .ToArray();

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                saltBytes,
                iterations,
                hashAlgorithm,
                keySize);

            return Convert.ToHexString(hash);
        }
    }
}
