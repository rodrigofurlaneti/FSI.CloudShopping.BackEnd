namespace FSI.CloudShopping.Domain.ValueObjects;

public record Money
{
    public decimal Value { get; }

    public Money(decimal value)
    {
        Value = value;
    }

    public Money Add(Money other) => new(Value + other.Value);

    public Money Multiply(int quantity) => new(Value * quantity);
}