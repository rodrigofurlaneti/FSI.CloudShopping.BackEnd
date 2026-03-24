namespace FSI.CloudShopping.Domain.ValueObjects;

using System.Text.RegularExpressions;
using FSI.CloudShopping.Domain.Core;

/// <summary>
/// Value object representing a Brazilian phone number.
/// Accepts formats: (XX) 9XXXX-XXXX or (XX) XXXX-XXXX
/// </summary>
public class Phone : ValueObject
{
    public string Value { get; }

    public Phone(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Phone cannot be empty.", nameof(value));

        var cleaned = value.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");

        if (cleaned.Length < 10 || cleaned.Length > 11 || !cleaned.All(char.IsDigit))
            throw new ArgumentException("Invalid phone format. Expected Brazilian format.", nameof(value));

        Value = cleaned;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        if (Value.Length == 10)
            return $"({Value.Substring(0, 2)}) {Value.Substring(2, 4)}-{Value.Substring(6)}";

        return $"({Value.Substring(0, 2)}) {Value.Substring(2, 5)}-{Value.Substring(7)}";
    }
}
