using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Netlab.Helper;

public static class Seguridad
{
    public static byte[] HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    public static bool VerifyPassword(string plainPassword, byte[] hashedPasswordFromDb)
    {
        var inputHash = HashPassword(plainPassword);
        return inputHash.SequenceEqual(hashedPasswordFromDb);
    }
}
