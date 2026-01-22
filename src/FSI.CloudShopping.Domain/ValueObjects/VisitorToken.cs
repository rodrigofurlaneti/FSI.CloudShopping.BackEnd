namespace FSI.CloudShopping.Domain.ValueObjects
{
    public record VisitorToken
    {
        public Guid Value { get; }

        public VisitorToken(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentException("Visitor token cannot be empty.");

            Value = value;
        }

        public static VisitorToken NewToken() => new(Guid.NewGuid());
    }
}
