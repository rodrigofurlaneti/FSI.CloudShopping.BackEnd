using FSI.CloudShopping.Domain.Core;
namespace FSI.CloudShopping.Domain.ValueObjects
{
    public record Quantity
    {
        public int Value { get; }
        public Quantity(int value)
        {
            if (value < 1)
                throw new DomainException("Quantity must be at least 1.");
            Value = value;
        }
        public static implicit operator int(Quantity quantity) => quantity.Value;
        public override string ToString() => Value.ToString();
    }
}