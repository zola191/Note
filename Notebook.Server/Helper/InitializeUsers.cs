using Microsoft.EntityFrameworkCore;
using Notebook.Server.Domain;
using Notebook.Server.Enum;
using System.Security.Cryptography;
using System.Text;

namespace Notebook.Server.Helper
{
    public static class InitializeUsers
    {
        private const int keySize = 64;
        private const int iterations = 350000;
        private static HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;

        private static string CreateSalt()
        {
            var salt = RandomNumberGenerator.GetBytes(keySize);
            return Convert.ToHexString(salt);
        }

        private static string HashPassword(string password, string salt)
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

        public static User CreateAdmin(string email, string password)
        {
            var salt = CreateSalt();
            var admin = new User()
            {
                Email = email,
                PasswordHash = HashPassword(password, salt),
                Salt = salt,
            };
            return admin;
        }
    }
}
