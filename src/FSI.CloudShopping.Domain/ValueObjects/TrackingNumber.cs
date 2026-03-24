namespace FSI.CloudShopping.Domain.ValueObjects;

using FSI.CloudShopping.Domain.Core;

/// <summary>
/// Value object representing a shipping tracking number.
/// </summary>
public class TrackingNumber : ValueObject
{
    public string Value { get; }

    public TrackingNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Tracking number cannot be empty.", nameof(value));

        var cleaned = value.Trim().ToUpperInvariant();

        if (cleaned.Length < 8 || cleaned.Length > 50)
            throw new ArgumentException("Tracking number must be between 8 and 50 characters.", nameof(value));

        Value = cleaned;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
