using FSI.CloudShopping.Domain.Core;
namespace FSI.CloudShopping.Domain.ValueObjects
{
    public record Money
    {
        public decimal Value { get; set; }
        public Money(decimal value)
        {
            if (value < 0)
                throw new DomainException("O valor monetário não pode ser negativo.");

            Value = value;
        }
        public Money Add(Money other) => new(Value + other.Value);
        public Money Multiply(int quantity)
        {
            if (quantity < 0)
                throw new DomainException("A quantidade para multiplicação não pode ser negativa.");

            return new(Value * quantity);
        }
        public static Money Zero => new(0);
    }
}

