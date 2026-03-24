namespace FSI.CloudShopping.Infrastructure.Security;

using BCrypt.Net;

public class PasswordHasher
{
    public static string HashPassword(string password)
    {
        var salt = BCrypt.GenerateSalt(12);
        return BCrypt.HashPassword(password, salt);
    }

    public static bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Verify(password, hash);
    }

    public static string GenerateSalt()
    {
        return BCrypt.GenerateSalt(12);
    }
}
