using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Helper
{
    public class PasswordHasherBCrypt
    {
         // Şifreyi hashle
    public static string HashPassword(string password)
    {
        // BCrypt ile hash işlemi
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    // Şifreyi doğrula
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        // Hash'i verilen şifre ile doğrula
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
    }
}