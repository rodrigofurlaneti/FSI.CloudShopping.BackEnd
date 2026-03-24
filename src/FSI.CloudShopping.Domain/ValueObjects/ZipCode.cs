namespace FSI.CloudShopping.Domain.ValueObjects;

using FSI.CloudShopping.Domain.Core;

/// <summary>
/// Value object representing a Brazilian postal code (CEP) - 8 digits.
/// </summary>
public class ZipCode : ValueObject
{
    public string Value { get; }

    public ZipCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("ZipCode cannot be empty.", nameof(value));

        var cleaned = value.Replace("-", "").Replace(".", "");

        if (cleaned.Length != 8 || !cleaned.All(char.IsDigit))
            throw new ArgumentException("ZipCode must contain exactly 8 digits.", nameof(value));

        Value = cleaned;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => $"{Value.Substring(0, 5)}-{Value.Substring(5)}";
}
