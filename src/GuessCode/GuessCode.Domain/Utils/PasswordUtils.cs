using System.Security.Cryptography;

namespace GuessCode.Domain.Utils;

public static class PasswordUtils
{
    public static string GetHash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(
            password,
            BCrypt.Net.BCrypt.GenerateSalt(12));
    }

    public static bool VerifyPassword(string hash, string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
    
    public static string GeneratePassword(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var password = new char[length];
        var randomBytes = new byte[length];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        for (var i = 0; i < length; i++)
        {
            password[i] = chars[randomBytes[i] % chars.Length];
        }

        return new string(password);
    }
}