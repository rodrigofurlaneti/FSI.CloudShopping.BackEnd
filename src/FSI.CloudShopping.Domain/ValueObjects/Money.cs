namespace FSI.CloudShopping.Domain.ValueObjects;

using FSI.CloudShopping.Domain.Core;

/// <summary>
/// Value object representing monetary amounts with support for common operations.
/// </summary>
public class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency = "BRL")
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));

        if (string.IsNullOrWhiteSpace(currency) || currency.Length != 3)
            throw new ArgumentException("Currency must be a 3-letter code.", nameof(currency));

        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }

    public Money Add(Money other)
    {
        if (other.Currency != Currency)
            throw new InvalidOperationException($"Cannot add amounts in different currencies: {Currency} and {other.Currency}");

        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        if (other.Currency != Currency)
            throw new InvalidOperationException($"Cannot subtract amounts in different currencies: {Currency} and {other.Currency}");

        var result = Amount - other.Amount;
        if (result < 0)
            throw new InvalidOperationException("Subtraction would result in negative amount.");

        return new Money(result, Currency);
    }

    public Money Multiply(decimal factor)
    {
        if (factor < 0)
            throw new ArgumentException("Multiplication factor cannot be negative.", nameof(factor));

        return new Money(Amount * factor, Currency);
    }

    public Money Divide(decimal divisor)
    {
        if (divisor <= 0)
            throw new ArgumentException("Divisor must be greater than zero.", nameof(divisor));

        return new Money(Amount / divisor, Currency);
    }

    public bool IsGreaterThan(Money other)
    {
        if (other.Currency != Currency)
            throw new InvalidOperationException($"Cannot compare amounts in different currencies: {Currency} and {other.Currency}");

        return Amount > other.Amount;
    }

    public bool IsGreaterThanOrEqual(Money other)
    {
        if (other.Currency != Currency)
            throw new InvalidOperationException($"Cannot compare amounts in different currencies: {Currency} and {other.Currency}");

        return Amount >= other.Amount;
    }

    public bool IsLessThan(Money other)
    {
        if (other.Currency != Currency)
            throw new InvalidOperationException($"Cannot compare amounts in different currencies: {Currency} and {other.Currency}");

        return Amount < other.Amount;
    }

    public bool IsLessThanOrEqual(Money other)
    {
        if (other.Currency != Currency)
            throw new InvalidOperationException($"Cannot compare amounts in different currencies: {Currency} and {other.Currency}");

        return Amount <= other.Amount;
    }

    public bool IsZero => Amount == 0;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString() => $"{Amount:F2} {Currency}";

    public static Money operator +(Money left, Money right) => left.Add(right);
    public static Money operator -(Money left, Money right) => left.Subtract(right);
    public static Money operator *(Money left, decimal right) => left.Multiply(right);
    public static Money operator *(decimal left, Money right) => right.Multiply(left);
    public static bool operator >(Money left, Money right) => left.IsGreaterThan(right);
    public static bool operator >=(Money left, Money right) => left.IsGreaterThanOrEqual(right);
    public static bool operator <(Money left, Money right) => left.IsLessThan(right);
    public static bool operator <=(Money left, Money right) => left.IsLessThanOrEqual(right);
}
