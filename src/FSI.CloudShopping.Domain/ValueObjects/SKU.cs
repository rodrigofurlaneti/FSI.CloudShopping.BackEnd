namespace FSI.CloudShopping.Domain.ValueObjects;

using System.Text.RegularExpressions;
using FSI.CloudShopping.Domain.Core;

/// <summary>
/// Value object representing a product Stock Keeping Unit (SKU).
/// </summary>
public class SKU : ValueObject
{
    public string Value { get; }

    public SKU(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("SKU cannot be empty.", nameof(value));

        var cleaned = value.Trim().ToUpperInvariant();

        if (cleaned.Length < 3 || cleaned.Length > 50)
            throw new ArgumentException("SKU must be between 3 and 50 characters.", nameof(value));

        if (!Regex.IsMatch(cleaned, @"^[A-Z0-9\-_]+$"))
            throw new ArgumentException("SKU can only contain letters, numbers, hyphens, and underscores.", nameof(value));

        Value = cleaned;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
