using System;
using System.Security.Cryptography;
using System.Text;

namespace qrschool_deckstop.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            // return hex string to match stored hashes in database (lowercase)
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLowerInvariant();

        }
    }
}
