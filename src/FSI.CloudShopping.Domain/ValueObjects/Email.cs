namespace FSI.CloudShopping.Domain.ValueObjects;

using System.Text.RegularExpressions;
using FSI.CloudShopping.Domain.Core;

/// <summary>
/// Value object representing an email address with validation.
/// </summary>
public class Email : ValueObject
{
    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty.", nameof(value));

        if (!IsValidEmail(value))
            throw new ArgumentException("Email format is invalid.", nameof(value));

        Value = value.ToLowerInvariant();
    }

    private static bool IsValidEmail(string email)
    {
        const string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
