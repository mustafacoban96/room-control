using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace api.Helper
{
    public static class PasswordHasher256
    {
        public static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            // Şifreyi byte array'e çevir
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash işlemini uygula
            byte[] hashBytes = sha256.ComputeHash(passwordBytes);

            // Hash'i string'e çevir ve döndür
            return Convert.ToBase64String(hashBytes);
        }
    }
    }
}