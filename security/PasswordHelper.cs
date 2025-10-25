using System;
using System.Security.Cryptography;
using System.Text;

namespace SimuladorCajero.Security
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password, out string salt)
        {
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }

            salt = Convert.ToBase64String(saltBytes);

            string saltedPassword = password + salt;
            byte[] saltedBytes = Encoding.UTF8.GetBytes(saltedPassword);

            using (var sha = SHA256.Create())
            {
                byte[] hashBytes = sha.ComputeHash(saltedBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            string saltedPassword = enteredPassword + storedSalt;
            byte[] saltedBytes = Encoding.UTF8.GetBytes(saltedPassword);

            using (var sha = SHA256.Create())
            {
                byte[] hashBytes = sha.ComputeHash(saltedBytes);
                string enteredHash = Convert.ToBase64String(hashBytes);
                return enteredHash == storedHash;
            }
        }
    }
}
