namespace FSI.CloudShopping.Domain.ValueObjects;

using FSI.CloudShopping.Domain.Core;

/// <summary>
/// Value object representing a quantity (must be greater than 0).
/// </summary>
public class Quantity : ValueObject
{
    public int Value { get; }

    public Quantity(int value)
    {
        if (value <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(value));

        Value = value;
    }

    public Quantity Add(int amount)
    {
        var newValue = Value + amount;
        if (newValue <= 0)
            throw new InvalidOperationException("Quantity cannot be reduced below zero.");

        return new Quantity(newValue);
    }

    public Quantity Subtract(int amount)
    {
        var newValue = Value - amount;
        if (newValue <= 0)
            throw new InvalidOperationException("Quantity cannot be reduced below zero.");

        return new Quantity(newValue);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();

    public static implicit operator int(Quantity quantity) => quantity.Value;
    public static explicit operator Quantity(int value) => new(value);
}
