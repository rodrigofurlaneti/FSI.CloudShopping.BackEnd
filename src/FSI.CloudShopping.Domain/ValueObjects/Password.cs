namespace FSI.CloudShopping.Domain.ValueObjects;

using FSI.CloudShopping.Domain.Core;

/// <summary>
/// Value object representing a password with strength validation.
/// Stores hashed password and salt (hashing is delegated to infrastructure layer).
/// This class stores the hash but hashing/verification is done externally.
/// </summary>
public class Password : ValueObject
{
    public string Hash { get; }
    public string Salt { get; }

    public Password(string hash, string salt)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new ArgumentException("Password hash cannot be empty.", nameof(hash));

        if (string.IsNullOrWhiteSpace(salt))
            throw new ArgumentException("Password salt cannot be empty.", nameof(salt));

        Hash = hash;
        Salt = salt;
    }

    public static void ValidatePasswordStrength(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty.", nameof(password));

        if (password.Length < 8)
            throw new ArgumentException("Password must be at least 8 characters long.", nameof(password));

        if (!password.Any(char.IsUpper))
            throw new ArgumentException("Password must contain at least one uppercase letter.", nameof(password));

        if (!password.Any(char.IsLower))
            throw new ArgumentException("Password must contain at least one lowercase letter.", nameof(password));

        if (!password.Any(char.IsDigit))
            throw new ArgumentException("Password must contain at least one digit.", nameof(password));

        if (!password.Any(c => !char.IsLetterOrDigit(c)))
            throw new ArgumentException("Password must contain at least one special character.", nameof(password));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Hash;
        yield return Salt;
    }
}
