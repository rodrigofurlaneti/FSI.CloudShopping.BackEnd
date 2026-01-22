namespace FSI.CloudShopping.Domain.ValueObjects
{
    public record Quantity
    {
        public int Value { get; }

        public Quantity(int value)
        {
            if (value < 1)
                throw new ArgumentException("Quantity must be at least 1.");

            Value = value;
        }
    }
}
